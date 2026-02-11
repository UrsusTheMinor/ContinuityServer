using ContinuityServer.Application.Abstractions;
using ContinuityServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContinuityServer.Infrastructure.Repositories;

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