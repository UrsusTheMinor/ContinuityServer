using ContinuityServer.Application.Abstractions;
using ContinuityServer.Domain;
using ContinuityServer.Domain.Entities;

namespace ContinuityServer.Application;

public sealed record CreateVoiceChannelCommand(Guid GuildId, string Name);

public sealed class CreateVoiceChannelHandler
{
    private readonly IGuildRepository _guilds;
    private readonly IChannelRepository _channels;
    private readonly IUnitOfWork _uow;

    public CreateVoiceChannelHandler(IGuildRepository guilds, IChannelRepository channels, IUnitOfWork uow)
    {
        _guilds = guilds;
        _channels = channels;
        _uow = uow;
    }

    public async Task<Guid> HandleAsync(CreateVoiceChannelCommand cmd, CancellationToken ct)
    {
        var guild = await _guilds.GetAsync(cmd.GuildId, ct);
        if (guild is null) throw new InvalidOperationException("Guild not found.");

        var name = (cmd.Name ?? "").Trim();
        if (name.Length < 1) throw new ArgumentException("Channel name too short.");

        var ch = new Channel
        {
            Id = Guid.NewGuid(),
            GuildId = cmd.GuildId,
            Name = name,
            Type = ChannelType.Voice
        };

        await _channels.AddAsync(ch, ct);
        await _uow.SaveChangesAsync(ct);
        return ch.Id;
    }
}