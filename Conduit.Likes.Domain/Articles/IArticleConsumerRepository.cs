using Conduit.Shared.Events.Models.Articles.CreateArticle;
using Conduit.Shared.Events.Models.Articles.DeleteArticle;
using Conduit.Shared.Events.Models.Articles.UpdateArticle;

namespace Conduit.Likes.Domain.Articles;

public interface IArticleConsumerRepository
{
    Task CreateAsync(CreateArticleEventModel model);
    Task RemoveAsync(DeleteArticleEventModel model);
    Task UpdateAsync(UpdateArticleEventModel model);
}
