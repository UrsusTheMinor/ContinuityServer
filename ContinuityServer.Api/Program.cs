using ContinuityServer.Api;
using ContinuityServer.Application;
using ContinuityServer.Application.Abstractions;
using ContinuityServer.Infrastructure;
using ContinuityServer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using ChatRepository = ContinuityServer.Infrastructure.ChatRepository;
using EfUnitOfWork = ContinuityServer.Infrastructure.EfUnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("db"),
        sql =>
        {
            sql.MigrationsAssembly("ContinuityServer.Infrastructure");
            sql.EnableRetryOnFailure();
        });
});
*/
var cs = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    if (!string.IsNullOrWhiteSpace(cs) && cs.Contains("(localdb)", StringComparison.OrdinalIgnoreCase))
    {
        // Local dev on Windows (optional)
        opt.UseSqlServer(cs);
    }
    else if (!string.IsNullOrWhiteSpace(cs) && (cs.Contains("Server=", StringComparison.OrdinalIgnoreCase) || cs.Contains("Data Source=", StringComparison.OrdinalIgnoreCase)))
    {
        // If you provide a real SQL Server connection string, keep SQL Server
        opt.UseSqlServer(cs);
    }
    else
    {
        // Fallback: SQLite (great for Linux server)
        opt.UseSqlite("Data Source=/opt/continuity/data/continuity.db");
    }
});

builder.Services.AddScoped<IGuildRepository, GuildRepository>();
builder.Services.AddScoped<IChannelRepository, ChannelRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

builder.Services.AddScoped<CreateGuildHandler>();
builder.Services.AddScoped<CreateTextChannelHandler>();
builder.Services.AddScoped<PostMessageHandler>();
builder.Services.AddScoped<CreateVoiceChannelHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");


app.Run();