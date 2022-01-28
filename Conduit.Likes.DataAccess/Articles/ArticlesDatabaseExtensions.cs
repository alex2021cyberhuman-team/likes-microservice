using Conduit.Likes.Domain.Articles;
using StackExchange.Redis;

namespace Conduit.Likes.DataAccess.Articles;

public static class ArticlesDatabaseExtensions
{
    public static async Task<ArticleModel> FindArticleAsync(
        this IDatabase database,
        string articleSlug)
    {
        var articleRedisValues = await database.HashGetAsync(
            ArticlesKeys.GetArticleKey(articleSlug),
            ArticlesKeys.GetArticleFields());
        var articleModel = articleRedisValues.Convert();
        return articleModel;
    }
}