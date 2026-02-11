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
var cs = builder.Configuration.GetConnectionString("Default")
         ?? throw new InvalidOperationException("Missing ConnectionStrings:Default");

builder.Services.AddDbContext<AppDbContext>(o =>
{
    // SQLite connection string examples:
    // "Data Source=/opt/continuity/data/continuity.db"
    // "/opt/continuity/data/continuity.db"
    if (cs.Contains("Data Source=", StringComparison.OrdinalIgnoreCase) ||
        cs.EndsWith(".db", StringComparison.OrdinalIgnoreCase))
    {
        o.UseSqlite(cs);
    }
    else
    {
        o.UseSqlServer(cs);
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

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");


app.Run();