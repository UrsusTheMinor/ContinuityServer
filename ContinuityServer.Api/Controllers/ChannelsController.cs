using ContinuityServer.Application;
using ContinuityServer.Application.Abstractions;
using ContinuityServer.Contracts.Dtos.Chat;
using Microsoft.AspNetCore.Mvc;

namespace ContinuityServer.Api.Controllers;

[ApiController]
[Route("api/channels")]
public sealed class ChannelsController : ControllerBase
{
    private readonly CreateTextChannelHandler _createText;
    private readonly IChannelRepository _channels;
    private readonly CreateVoiceChannelHandler _createVoice;


    public ChannelsController(CreateTextChannelHandler createText, CreateVoiceChannelHandler createVoice, IChannelRepository channels)
    {
        _createText = createText;
        _channels = channels;
        _createVoice = createVoice;

    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ChannelDto>>> GetAll(CancellationToken ct)
    {
        var all = await _channels.GetAllAsync(ct);
        return Ok(all.Select(c => new ChannelDto(c.Id, c.GuildId, c.Name, (int)c.Type)).ToList());
    }

    [HttpGet("{channelId:guid}")]
    public async Task<ActionResult<ChannelDto>> GetOne(Guid channelId, CancellationToken ct)
    {
        var c = await _channels.GetAsync(channelId, ct);
        if (c is null) return NotFound();

        return Ok(new ChannelDto(c.Id, c.GuildId, c.Name, (int)c.Type));
    }

    [HttpPost("text")]
    public async Task<ActionResult<ChannelDto>> CreateText([FromBody] CreateChannelRequest req, CancellationToken ct)
    {
        var id = await _createText.HandleAsync(new CreateTextChannelCommand(req.GuildId, req.Name), ct);
        return Ok(new ChannelDto(id, req.GuildId, req.Name.Trim(), (int)Domain.ChannelType.Text));
    }
    
    [HttpPost("voice")]
    public async Task<ActionResult<ChannelDto>> CreateVoice([FromBody] CreateChannelRequest req, CancellationToken ct)
    {
        var id = await _createVoice.HandleAsync(new CreateVoiceChannelCommand(req.GuildId, req.Name), ct);
        return Ok(new ChannelDto(id, req.GuildId, req.Name.Trim(), (int)Domain.ChannelType.Voice));
    }
}