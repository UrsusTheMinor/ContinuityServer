namespace ContinuityServer.Contracts.Dtos.Chat;

public sealed record CreateChannelRequest(Guid GuildId, string Name);
