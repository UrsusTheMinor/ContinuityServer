using System.Net.Sockets;
using ContinuityServer.Voice.Protocol;
using System.Net;

namespace ContinuityServer.Voice.Transport;

public sealed class UdpVoiceTransport : IVoiceTransport, IDisposable
{
    private readonly UdpClient _udp;

    public UdpVoiceTransport(int port)
    {
        _udp = new UdpClient(port);
        _udp.Client.ReceiveBufferSize = 1_000_000;
        _udp.Client.SendBufferSize = 1_000_000;
    }

    public async Task RunAsync(Func<VoicePacket, Task> onPacket, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            UdpReceiveResult received;

            try
            {
                received = await _udp.ReceiveAsync(ct);
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset)
            {
                // Windows UDP ICMP "port unreachable" - ignore
                continue;
            }

            if (VoicePacketParser.TryParse(received, out var packet))
            {
                await onPacket(packet);
            }
        }
    }

    public ValueTask<int> SendAsync(byte[] datagram, IPEndPoint recipient, CancellationToken ct)
        => _udp.SendAsync(datagram.AsMemory(), recipient, ct);


    public void Dispose() => _udp.Dispose();
}