namespace ContinuityServer.Voice.Sessions;

public interface IVoiceSessionStore
{
    void Upsert(Guid userId, Guid channelId, System.Net.IPEndPoint endPoint);
    IReadOnlyList<VoiceSession> GetByChannel(Guid channelId);
    void RemoveStale(TimeSpan maxAge);
}