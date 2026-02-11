using ContinuityServer.Application.Abstractions;
using ContinuityServer.Domain.Entities;

namespace ContinuityServer.Infrastructure;

using ContinuityServer.Application;
using ContinuityServer.Domain;
using Microsoft.EntityFrameworkCore;

public sealed class ChatRepository : IChatRepository
{
    private readonly AppDbContext _db;
    public ChatRepository(AppDbContext db) => _db = db;

    public Task AddMessageAsync(ChatMessage msg, CancellationToken ct)
        => _db.Messages.AddAsync(msg, ct).AsTask();

    public async Task<IReadOnlyList<ChatMessage>> GetLatestAsync(Guid channelId, int take, CancellationToken ct)
        => await _db.Messages
            .Where(m => m.ChannelId == channelId)
            .OrderByDescending(m => m.CreatedAtUtc)
            .Take(take)
            .ToListAsync(ct);
}

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    public EfUnitOfWork(AppDbContext db) => _db = db;
    public Task<int> SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
