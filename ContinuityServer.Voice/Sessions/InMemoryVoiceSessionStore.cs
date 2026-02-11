using System.Collections.Concurrent;
using System.Net;

namespace ContinuityServer.Voice.Sessions;

public sealed class InMemoryVoiceSessionStore : IVoiceSessionStore
{
    private readonly ConcurrentDictionary<Guid, VoiceSession> _sessions = new();

    public void Upsert(Guid userId, Guid channelId, IPEndPoint endPoint)
    {
        _sessions.AddOrUpdate(
            userId,
            _ => new VoiceSession { UserId = userId, ChannelId = channelId, EndPoint = endPoint, LastSeen = DateTimeOffset.UtcNow },
            (_, s) =>
            {
                s.ChannelId = channelId;
                s.EndPoint = endPoint;
                s.LastSeen = DateTimeOffset.UtcNow;
                return s;
            });
    }

    public IReadOnlyList<VoiceSession> GetByChannel(Guid channelId)
        => _sessions.Values.Where(s => s.ChannelId == channelId).ToList();

    public void RemoveStale(TimeSpan maxAge)
    {
        var cutoff = DateTimeOffset.UtcNow - maxAge;
        foreach (var kv in _sessions)
        {
            if (kv.Value.LastSeen < cutoff)
                _sessions.TryRemove(kv.Key, out _);
        }
    }
}