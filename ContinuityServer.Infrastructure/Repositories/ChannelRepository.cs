using ContinuityServer.Application.Abstractions;
using ContinuityServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContinuityServer.Infrastructure.Repositories;

public sealed class ChannelRepository : IChannelRepository
{
    private readonly AppDbContext _db;
    public ChannelRepository(AppDbContext db) => _db = db;

    public Task AddAsync(Channel channel, CancellationToken ct)
        => _db.Channels.AddAsync(channel, ct).AsTask();

    public Task<Channel?> GetAsync(Guid id, CancellationToken ct)
        => _db.Channels.FirstOrDefaultAsync(x => x.Id == id, ct);
    
    public async Task<IReadOnlyList<Channel>> GetAllAsync(CancellationToken ct)
        => await _db.Channels
            .OrderBy(c => c.GuildId)
            .ThenBy(c => c.Name)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Channel>> GetByGuildAsync(Guid guildId, CancellationToken ct)
        => await _db.Channels
            .Where(c => c.GuildId == guildId)
            .OrderBy(c => c.Name)
            .ToListAsync(ct);

}