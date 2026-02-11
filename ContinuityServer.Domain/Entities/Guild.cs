namespace ContinuityServer.Domain.Entities;

public sealed class Guild
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public List<Channel> Channels { get; set; } = new();
}