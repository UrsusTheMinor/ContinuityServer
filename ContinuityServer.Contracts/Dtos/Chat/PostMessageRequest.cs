namespace ContinuityServer.Contracts.Dtos.Chat;

public sealed record PostMessageRequest(Guid ChannelId, Guid AuthorUserId, string Content);
