namespace ContinuityServer.Contracts.Dtos.Chat;

public sealed record MessageDto(Guid Id, Guid ChannelId, Guid AuthorUserId, string Content, DateTimeOffset CreatedAt);
