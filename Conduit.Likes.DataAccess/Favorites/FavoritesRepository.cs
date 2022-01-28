using Conduit.Likes.Domain.Favorites;

namespace Conduit.Likes.DataAccess.Favorites;

public class FavoritesRepository : IFavoritesRepository
{
    public Task<bool> AddAsync(
        Guid articleId,
        Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(
        Guid articleId,
        Guid userId)
    {
        throw new NotImplementedException();
    }
}
