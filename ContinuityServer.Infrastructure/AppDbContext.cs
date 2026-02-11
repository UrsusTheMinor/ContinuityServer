using ContinuityServer.Domain.Entities;

namespace ContinuityServer.Infrastructure;

using Microsoft.EntityFrameworkCore;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Guild> Guilds => Set<Guild>();
    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<ChatMessage> Messages => Set<ChatMessage>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Guild>()
            .Property(x => x.Name)
            .HasMaxLength(200);

        b.Entity<Channel>()
            .Property(x => x.Name)
            .HasMaxLength(200);

        b.Entity<ChatMessage>()
            .Property(x => x.Content)
            .HasMaxLength(4000);

        b.Entity<Guild>()
            .HasMany(g => g.Channels)
            .WithOne()
            .HasForeignKey(c => c.GuildId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<ChatMessage>().HasIndex(x => new { x.ChannelId, x.CreatedAt });
    }
}
