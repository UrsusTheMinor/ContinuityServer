using Microsoft.AspNetCore.SignalR;

namespace ContinuityServer.Api;

public sealed class ChatHub : Hub
{
    // Join a specific text channel (SignalR group)
    public Task JoinChannel(string channelId)
        => Groups.AddToGroupAsync(Context.ConnectionId, channelId);

    public Task LeaveChannel(string channelId)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, channelId);
}