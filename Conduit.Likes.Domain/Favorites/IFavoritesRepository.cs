namespace Conduit.Likes.Domain.Favorites;

public interface IFavoritesRepository
{
    Task<bool> AddAsync(
        Guid articleId,
        Guid userId);
    
    Task<bool> RemoveAsync(
        Guid articleId,
        Guid userId);
}
