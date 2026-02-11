using System.Net;
using System.Net.Sockets;

namespace ContinuityServer.Voice.Protocol;

public static class VoicePacketParser
{
    // v2 header: 1 + 16 + 16 + 4 + 4 = 41 bytes
    public const byte Version = 2;
    public const int HeaderSize = 41;

    public static bool TryParse(UdpReceiveResult received, out VoicePacket packet)
    {
        var data = received.Buffer;
        packet = default;

        if (data is null || data.Length < HeaderSize) return false;
        if (data[0] != Version) return false;

        var userId = new Guid(data.AsSpan(1, 16));
        var channelId = new Guid(data.AsSpan(17, 16));

        uint seq = ReadU32LE(data, 33);
        uint ts  = ReadU32LE(data, 37);

        var payloadLen = data.Length - HeaderSize;
        if (payloadLen <= 0) return false;

        // Copy payload so it survives after buffer reuse (safe + simple)
        var payload = new byte[payloadLen];
        Buffer.BlockCopy(data, HeaderSize, payload, 0, payloadLen);

        packet = new VoicePacket(
            userId,
            channelId,
            seq,
            ts,
            payload,
            received.RemoteEndPoint
        );

        return true;
    }

    private static uint ReadU32LE(byte[] b, int offset)
        => (uint)(b[offset]
                  | (b[offset + 1] << 8)
                  | (b[offset + 2] << 16)
                  | (b[offset + 3] << 24));
}