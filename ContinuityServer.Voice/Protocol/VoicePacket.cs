using System.Net;

namespace ContinuityServer.Voice.Protocol;

public readonly record struct VoicePacket(
    Guid UserId,
    Guid ChannelId,
    uint Sequence,
    uint Timestamp,
    ReadOnlyMemory<byte> OpusPayload,
    IPEndPoint RemoteEndPoint
);