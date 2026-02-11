using ContinuityServer.Application;
using ContinuityServer.Application.Abstractions;
using ContinuityServer.Contracts.Dtos.Chat;
using Microsoft.AspNetCore.Mvc;

namespace ContinuityServer.Api.Controllers;

[ApiController]
[Route("api/guilds")]
public sealed class GuildsController : ControllerBase
{
    private readonly CreateGuildHandler _create;
    private readonly IGuildRepository _guilds;
    private readonly IChannelRepository _channels;

    public GuildsController(CreateGuildHandler create, IGuildRepository guilds, IChannelRepository channels)
    {
        _create = create;
        _guilds = guilds;
        _channels = channels;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GuildDto>>> GetAll(CancellationToken ct)
    {
        var all = await _guilds.GetAllAsync(ct);
        return Ok(all.Select(g => new GuildDto(g.Id, g.Name)).ToList());
    }

    [HttpGet("{guildId:guid}")]
    public async Task<ActionResult<GuildDto>> GetOne(Guid guildId, CancellationToken ct)
    {
        var g = await _guilds.GetAsync(guildId, ct);
        if (g is null) return NotFound();

        return Ok(new GuildDto(g.Id, g.Name));
    }

    [HttpGet("{guildId:guid}/channels")]
    public async Task<ActionResult<IReadOnlyList<ChannelDto>>> GetChannels(Guid guildId, CancellationToken ct)
    {
        var g = await _guilds.GetAsync(guildId, ct);
        if (g is null) return NotFound("Guild not found.");

        var chans = await _channels.GetByGuildAsync(guildId, ct);
        var dto = chans.Select(c => new ChannelDto(c.Id, c.GuildId, c.Name, (int)c.Type)).ToList();

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<GuildDto>> Create([FromBody] CreateGuildRequest req, CancellationToken ct)
    {
        var id = await _create.HandleAsync(new CreateGuildCommand(req.Name), ct);
        return Ok(new GuildDto(id, req.Name.Trim()));
    }
}