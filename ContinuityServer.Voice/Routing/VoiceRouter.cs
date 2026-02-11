using ContinuityServer.Voice.Protocol;
using ContinuityServer.Voice.Sessions;
using Microsoft.Extensions.Options;
using ContinuityServer.Voice.Configuration;
using System.Net;

namespace ContinuityServer.Voice.Routing;

public sealed class VoiceRouter : IVoiceRouter
{
    private readonly IVoiceSessionStore _store;
    private readonly VoiceOptions _opt;

    public VoiceRouter(IVoiceSessionStore store, IOptions<VoiceOptions> opt)
    {
        _store = store;
        _opt = opt.Value;
    }

    public IReadOnlyList<IPEndPoint> GetRecipients(VoicePacket packet)
    {
        var sessions = _store.GetByChannel(packet.ChannelId);

        var recipients = sessions
            .Select(s => s.EndPoint)
            .ToList();

        if (!_opt.EchoToSender)
        {
            recipients = recipients
                .Where(ep => !ep.Equals(packet.RemoteEndPoint))
                .ToList();
        }

        return recipients;
    }
}