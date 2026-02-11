namespace ContinuityServer.Domain.Entities;

public sealed class Guild
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public List<Channel> Channels { get; set; } = new();
}