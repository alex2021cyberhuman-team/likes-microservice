using Conduit.Likes.Domain.Articles;
using StackExchange.Redis;

namespace Conduit.Likes.DataAccess.Articles;

public static class ArticlesKeys
{
    public const string ArticleId = "i";
    public const string ArticleSlug = "s";

    static ArticlesKeys()
    {
        _articleFields = new RedisValue[] { ArticleId, ArticleSlug };
    }

    private static readonly RedisValue[] _articleFields;

    public static RedisKey GetArticleSlugKey(
        string articleSlug)
    {
        return $"articles:slug:{articleSlug}";
    }

    public static RedisValue[] GetArticleFields()
    {
        return _articleFields;
    }

    public static ArticleModel Convert(
        this RedisValue[] articlesRedisValues)
    {
        return new()
        {
            Id = Guid.Parse(articlesRedisValues[0]),
            Slug = articlesRedisValues[1]
        };
    }

    public static RedisKey GetArticleIdKey(
        Guid articleId)
    {
        return $"articles:id:{articleId:N}";
    }
}
