namespace ContinuityServer.Voice.Configuration;

public sealed class VoiceOptions
{
    public int UdpPort { get; set; } = 40000;

    // If true: send a user's own voice back to them (solo test)
    public bool EchoToSender { get; set; } = true;

    // Remove sessions not seen for this many seconds
    public int SessionTimeoutSeconds { get; set; } = 30;
}