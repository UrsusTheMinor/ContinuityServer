using ContinuityServer.Domain.Entities;

namespace ContinuityServer.Application.Abstractions;

public interface IGuildRepository
{
    Task AddAsync(Guild guild, CancellationToken ct);
    Task<Guild?> GetAsync(Guid id, CancellationToken ct);

    Task<IReadOnlyList<Guild>> GetAllAsync(CancellationToken ct);
}