using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace REST.Hubs;

/// <summary>
/// SignalR hub for real-time exchange rate updates.
/// Requires JWT authentication (Consumer or Admin role).
/// </summary>
[Authorize(Roles = "Consumer,Admin")]
public class ExchangeRatesHub : Hub
{
    /// <summary>
    /// Called when a client connects to the hub.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        var userName = Context.User?.Identity?.Name;

        await Clients.Caller.SendAsync("Connected", new
        {
            Message = "Successfully connected to Exchange Rates Hub",
            ConnectionId = Context.ConnectionId,
            UserId = userId,
            UserName = userName
        });

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Called when a client disconnects from the hub.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
