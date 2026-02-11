using ContinuityServer.Application.Abstractions;
using ContinuityServer.Domain;
using ContinuityServer.Domain.Entities;

namespace ContinuityServer.Application;

public sealed class PostMessageHandler
{
    private readonly IChannelRepository _channels;
    private readonly IChatRepository _chat;
    private readonly IUnitOfWork _uow;

    public PostMessageHandler(IChannelRepository channels, IChatRepository chat, IUnitOfWork uow)
    {
        _channels = channels;
        _chat = chat;
        _uow = uow;
    }

    public async Task<ChatMessage> HandleAsync(PostMessageCommand cmd, CancellationToken ct)
    {
        var channel = await _channels.GetAsync(cmd.ChannelId, ct);
        if (channel is null) throw new InvalidOperationException("Channel not found.");
        if (channel.Type != ChannelType.Text) throw new InvalidOperationException("Not a text channel.");

        var content = (cmd.Content ?? "").Trim();
        if (content.Length == 0) throw new ArgumentException("Message content empty.");
        if (content.Length > 4000) throw new ArgumentException("Message too long (max 4000).");

        var msg = new ChatMessage
        {
            Id = Guid.NewGuid(),
            ChannelId = cmd.ChannelId,
            AuthorUserId = cmd.AuthorUserId,
            Content = content,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _chat.AddMessageAsync(msg, ct);
        await _uow.SaveChangesAsync(ct);
        return msg;
    }
}
