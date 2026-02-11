
using ContinuityServer.Domain.Entities;

namespace ContinuityServer.Application.Abstractions;

public interface IChannelRepository
{
    Task AddAsync(Channel channel, CancellationToken ct);
    Task<Channel?> GetAsync(Guid id, CancellationToken ct);

    Task<IReadOnlyList<Channel>> GetByGuildAsync(Guid guildId, CancellationToken ct);
    Task<IReadOnlyList<Channel>> GetAllAsync(CancellationToken ct);
}