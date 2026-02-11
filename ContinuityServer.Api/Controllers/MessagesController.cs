using ContinuityServer.Application;
using ContinuityServer.Application.Abstractions;
using ContinuityServer.Contracts.Dtos.Chat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ContinuityServer.Api.Controllers;

[ApiController]
[Route("api/messages")]
public sealed class MessagesController : ControllerBase
{
    private readonly PostMessageHandler _post;
    private readonly IChatRepository _chat;
    private readonly IHubContext<ChatHub> _hub;

    public MessagesController(PostMessageHandler post, IChatRepository chat, IHubContext<ChatHub> hub)
    {
        _post = post;
        _chat = chat;
        _hub = hub;
    }

    [HttpGet("{channelId:guid}")]
    public async Task<ActionResult<IReadOnlyList<MessageDto>>> GetLatest(Guid channelId, [FromQuery] int take = 50, CancellationToken ct = default)
    {
        take = Math.Clamp(take, 1, 200);
        var msgs = await _chat.GetLatestAsync(channelId, take, ct);

        var dto = msgs
            .OrderBy(m => m.CreatedAtUtc) // return chronological
            .Select(m => new MessageDto(m.Id, m.ChannelId, m.AuthorUserId, m.Content, m.CreatedAtUtc))
            .ToList();

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> Post([FromBody] PostMessageRequest req, CancellationToken ct)
    {
        var msg = await _post.HandleAsync(new PostMessageCommand(req.ChannelId, req.AuthorUserId, req.Content), ct);

        var dto = new MessageDto(msg.Id, msg.ChannelId, msg.AuthorUserId, msg.Content, msg.CreatedAtUtc);

        // Broadcast to clients that joined this channel group
        await _hub.Clients.Group(req.ChannelId.ToString()).SendAsync("MessagePosted", new MessagePosted(dto), ct);

        return Ok(dto);
    }
}