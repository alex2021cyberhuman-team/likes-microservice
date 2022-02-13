using Conduit.Likes.Domain.Favorites;
using Conduit.Likes.Domain.Shared;

namespace Conduit.Likes.DataAccess.Favorites;

public class FavoritesRepository : IFavoritesRepository
{
    private readonly ConnectionProvider _connectionProvider;

    public FavoritesRepository(
        ConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<Error> AddAsync(
        Guid articleId,
        Guid userId)
    {
        var database = await _connectionProvider.GetDatabaseAsync();
        var transaction = database.CreateTransaction();
        var added = await transaction.SetAddAsync(
            FavoritesKeys.GetUserFavoritesKey(userId), articleId.ToString("N"));
        await transaction.ExecuteAsync();
        return added ? Error.None : Error.BadRequest;
    }

    public async Task<Error> RemoveAsync(
        Guid articleId,
        Guid userId)
    {
        var database = await _connectionProvider.GetDatabaseAsync();
        var transaction = database.CreateTransaction();
        var removed = await transaction.SetRemoveAsync(
            FavoritesKeys.GetUserFavoritesKey(userId), articleId.ToString("N"));
        await transaction.ExecuteAsync();
        return removed ? Error.None : Error.BadRequest;
    }
}
