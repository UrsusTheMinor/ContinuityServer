using ContinuityServer.Application.Abstractions;
using ContinuityServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContinuityServer.Infrastructure.Repositories;

public sealed class GuildRepository : IGuildRepository
{
    private readonly AppDbContext _db;
    public GuildRepository(AppDbContext db) => _db = db;

    public Task AddAsync(Guild guild, CancellationToken ct)
        => _db.Guilds.AddAsync(guild, ct).AsTask();

    public Task<Guild?> GetAsync(Guid id, CancellationToken ct)
        => _db.Guilds.FirstOrDefaultAsync(x => x.Id == id, ct);
    
    public async Task<IReadOnlyList<Guild>> GetAllAsync(CancellationToken ct)
        => await _db.Guilds
            .OrderBy(g => g.Name)
            .ToListAsync(ct);

}