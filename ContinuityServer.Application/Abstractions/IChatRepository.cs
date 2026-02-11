using ContinuityServer.Domain.Entities;

namespace ContinuityServer.Application.Abstractions;

public interface IChatRepository
{
    Task AddMessageAsync(ChatMessage msg, CancellationToken ct);
    Task<IReadOnlyList<ChatMessage>> GetLatestAsync(Guid channelId, int take, CancellationToken ct);
}