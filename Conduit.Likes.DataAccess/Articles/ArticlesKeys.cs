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

    public static RedisKey GetArticleKey(
        string articleSlug)
    {
        return $"articles:{articleSlug}";
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
}