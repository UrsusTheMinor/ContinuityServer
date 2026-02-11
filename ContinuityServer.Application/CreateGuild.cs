using ContinuityServer.Application.Abstractions;
using ContinuityServer.Domain.Entities;

namespace ContinuityServer.Application;

using ContinuityServer.Domain;

public sealed record CreateGuildCommand(string Name);

public sealed class CreateGuildHandler
{
    private readonly IGuildRepository _guilds;
    private readonly IUnitOfWork _uow;

    public CreateGuildHandler(IGuildRepository guilds, IUnitOfWork uow)
    {
        _guilds = guilds;
        _uow = uow;
    }

    public async Task<Guid> HandleAsync(CreateGuildCommand cmd, CancellationToken ct)
    {
        var name = (cmd.Name ?? "").Trim();
        if (name.Length < 2) throw new ArgumentException("Guild name too short.");

        var guild = new Guild { Id = Guid.NewGuid(), Name = name };
        await _guilds.AddAsync(guild, ct);
        await _uow.SaveChangesAsync(ct);
        return guild.Id;
    }
}
