namespace Conduit.Likes.Domain.FavoriteArticle;

public class FavoriteArticleRequest
{
    public Guid ArticleId { get; set; }

    public Guid UserId { get; set; }
}
