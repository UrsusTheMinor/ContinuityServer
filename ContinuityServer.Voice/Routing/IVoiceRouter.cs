using ContinuityServer.Voice.Protocol;

namespace ContinuityServer.Voice.Routing;

public interface IVoiceRouter
{
    IReadOnlyList<System.Net.IPEndPoint> GetRecipients(VoicePacket packet);
}