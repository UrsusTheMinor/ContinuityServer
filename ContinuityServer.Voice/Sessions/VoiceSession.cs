using System.Net;

namespace ContinuityServer.Voice.Sessions;

public sealed class VoiceSession
{
    public required Guid UserId { get; init; }
    public required Guid ChannelId { get; set; }
    public required IPEndPoint EndPoint { get; set; }
    public DateTimeOffset LastSeen { get; set; } = DateTimeOffset.UtcNow;
}