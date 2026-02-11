using ContinuityServer.Voice.Configuration;
using ContinuityServer.Voice.Protocol;
using ContinuityServer.Voice.Routing;
using ContinuityServer.Voice.Sessions;
using ContinuityServer.Voice.Transport;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ContinuityServer.Voice;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _log;
    private readonly IVoiceTransport _transport;
    private readonly IVoiceSessionStore _sessions;
    private readonly IVoiceRouter _router;
    private readonly VoiceOptions _opt;

    public Worker(
        ILogger<Worker> log,
        IVoiceTransport transport,
        IVoiceSessionStore sessions,
        IVoiceRouter router,
        IOptions<VoiceOptions> opt)
    {
        _log = log;
        _transport = transport;
        _sessions = sessions;
        _router = router;
        _opt = opt.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _log.LogInformation("Voice server started (UDP port {Port}).", _opt.UdpPort);

        // periodic cleanup
        _ = Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _sessions.RemoveStale(TimeSpan.FromSeconds(_opt.SessionTimeoutSeconds));
                await Task.Delay(2000, stoppingToken);
            }
        }, stoppingToken);

        await _transport.RunAsync(async packet =>
        {
            // Update session with last seen + endpoint + channel
            _sessions.Upsert(packet.UserId, packet.ChannelId, packet.RemoteEndPoint);

            // Route
            var recipients = _router.GetRecipients(packet);

            // Forward original datagram bytes:
            // Since parser copied payload, easiest is rebuild datagram with same header values + payload.
            var payload = packet.OpusPayload.ToArray();
            var datagram = VoicePacketBuilder.Build(packet.UserId, packet.ChannelId, packet.Sequence, packet.Timestamp, payload, payload.Length);

            foreach (var ep in recipients)
            {
                try { await _transport.SendAsync(datagram, ep, stoppingToken); }
                catch { /* ignore per-recipient send errors */ }
            }
        }, stoppingToken);
    }
}
