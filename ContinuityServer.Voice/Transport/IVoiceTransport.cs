using System.Net;
using ContinuityServer.Voice.Protocol;

namespace ContinuityServer.Voice.Transport;

public interface IVoiceTransport
{
    Task RunAsync(Func<VoicePacket, Task> onPacket, CancellationToken ct);
    ValueTask<int> SendAsync(byte[] datagram, IPEndPoint recipient, CancellationToken ct);

}