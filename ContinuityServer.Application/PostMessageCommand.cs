namespace ContinuityServer.Application;

public sealed record PostMessageCommand(Guid ChannelId, Guid AuthorUserId, string Content);
