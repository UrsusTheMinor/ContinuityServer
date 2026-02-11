namespace ContinuityServer.Contracts.Dtos.Chat;

public sealed record ChannelDto(Guid Id, Guid GuildId, string Name, int Type);
