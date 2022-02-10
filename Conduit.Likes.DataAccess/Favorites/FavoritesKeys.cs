using StackExchange.Redis;

namespace Conduit.Likes.DataAccess.Favorites;

public static class FavoritesKeys
{
    public static RedisKey GetUserFavoritesKey(Guid userId) => $"user:{userId:N}:favorites";
}
