using Conduit.Likes.BusinessLogic.Articles;
using Conduit.Likes.DataAccess;
using Conduit.Likes.DataAccess.Articles;
using Conduit.Likes.DataAccess.Favorites;
using Conduit.Likes.Domain.Articles;
using Conduit.Likes.Domain.Favorites;
using Conduit.Shared.Events.Models.Articles.CreateArticle;
using Conduit.Shared.Events.Models.Articles.DeleteArticle;
using Conduit.Shared.Events.Models.Articles.UpdateArticle;
using Conduit.Shared.Events.Models.Likes.Favorite;
using Conduit.Shared.Events.Models.Likes.Unfavorite;
using Conduit.Shared.Events.Services.RabbitMQ;
using Conduit.Shared.Startup;
using Conduit.Shared.Tokens;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

#region ServicesConfiguration

var services = builder.Services;
var environment = builder.Environment;
var configuration = builder.Configuration;

services.AddControllers();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new() { Title = "Conduit.Likes.WebApi", Version = "v1" });
});

services.AddJwtServices(configuration.GetSection("Jwt").Bind)
    .AddW3CLogging(configuration.GetSection("W3C").Bind)
    .AddHttpClient()
    .AddHttpContextAccessor()
    .RegisterRabbitMqWithHealthCheck(configuration.GetSection("RabbitMQ").Bind)
    .RegisterConsumer<CreateArticleEventModel, CreateArticleConsumer>(ConfigureConsumer)
    .RegisterConsumer<UpdateArticleEventModel, UpdateArticleConsumer>(ConfigureConsumer)
    .RegisterConsumer<DeleteArticleEventModel, DeleteArticleConsumer>(ConfigureConsumer)
    .RegisterProducer<FavoriteArticleEventModel>()
    .RegisterProducer<UnfavoriteArticleEventModel>()
    .AddSingleton<Conduit.Likes.Domain.Favorites.FavoriteArticle.FavoriteArticleHandler, Conduit.Likes.BusinessLogic.Unfavorites.FavoriteArticle.FavoriteArticleHandler>()
    .AddSingleton<Conduit.Likes.Domain.Favorites.UnfavoriteArticle.UnfavoriteArticleHandler, Conduit.Likes.BusinessLogic.Unfavorites.UnfavoriteArticle.UnfavoriteArticleHandler>()
    .AddSingleton<IFavoritesRepository, FavoritesRepository>()
    .AddSingleton<IArticleRepository, ArticlesRepository>()
    .AddSingleton<IArticleConsumerRepository, ArticleConsumerRepository>()
    .AddSingleton<ConnectionProvider>()
    .Configure<ConnectionProviderOptions>(configuration.GetSection("RedisDatabase").Bind);

#endregion

var app = builder.Build();

#region AppConfiguration

if (environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json",
            "Conduit.Likes.WebApi v1"));
    IdentityModelEventSource.ShowPII = true;
}

app.UseW3CLogging();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var initializationScope = app.Services.CreateScope();

await initializationScope.WaitHealthyServicesAsync(TimeSpan.FromHours(1));
await initializationScope.InitializeQueuesAsync();

#endregion

app.Run();

void ConfigureConsumer<T>(
    RabbitMqSettings<T> settings)
{
    settings.Consumer = "likes";
}
