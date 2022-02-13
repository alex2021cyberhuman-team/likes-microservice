using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Conduit.Likes.DataAccess;

public class ConnectionProvider
{
    private ConnectionMultiplexer? _connectionMultiplexer = null;

    private readonly IOptionsMonitor<ConnectionProviderOptions>
        _connectionProviderOptionsMonitor;

    public ConnectionProvider(
        IOptionsMonitor<ConnectionProviderOptions>
            connectionProviderOptionsMonitor)
    {
        _connectionProviderOptionsMonitor = connectionProviderOptionsMonitor;
    }

    public async Task<IDatabase> GetDatabaseAsync()
    {
        await ConnectAsync();
        return _connectionMultiplexer!.GetDatabase();
    }

    private async Task ConnectAsync()
    {
        var options = _connectionProviderOptionsMonitor.CurrentValue;
        if (_connectionMultiplexer is not { IsConnected: true })
        {
            _connectionMultiplexer =
                await ConnectionMultiplexer.ConnectAsync(options.Configuration);
        }
    }
}
