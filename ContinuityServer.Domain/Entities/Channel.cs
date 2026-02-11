namespace ContinuityServer.Domain.Entities;

public sealed class Channel
{
    public Guid Id { get; set; }
    public Guid GuildId { get; set; }
    public string Name { get; set; } = "";
    public ChannelType Type { get; set; }
}

