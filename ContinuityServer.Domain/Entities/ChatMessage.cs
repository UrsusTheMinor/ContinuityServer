namespace ContinuityServer.Domain.Entities;

public sealed class ChatMessage
{
    public Guid Id { get; set; }
    public Guid ChannelId { get; set; }
    public Guid AuthorUserId { get; set; }
    public string Content { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}