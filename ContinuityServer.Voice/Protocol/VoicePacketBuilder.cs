namespace ContinuityServer.Voice.Protocol;

public static class VoicePacketBuilder
{
    public static byte[] Build(Guid userId, Guid channelId, uint seq, uint ts, byte[] opus, int opusLen)
    {
        var buf = new byte[VoicePacketParser.HeaderSize + opusLen];
        buf[0] = VoicePacketParser.Version;

        userId.TryWriteBytes(buf.AsSpan(1, 16));
        channelId.TryWriteBytes(buf.AsSpan(17, 16));

        WriteU32LE(buf, 33, seq);
        WriteU32LE(buf, 37, ts);

        Buffer.BlockCopy(opus, 0, buf, VoicePacketParser.HeaderSize, opusLen);
        return buf;
    }

    private static void WriteU32LE(byte[] b, int offset, uint v)
    {
        b[offset + 0] = (byte)(v & 0xFF);
        b[offset + 1] = (byte)((v >> 8) & 0xFF);
        b[offset + 2] = (byte)((v >> 16) & 0xFF);
        b[offset + 3] = (byte)((v >> 24) & 0xFF);
    }
}