using ContinuityServer.Voice;
using ContinuityServer.Voice.Configuration;
using ContinuityServer.Voice.Routing;
using ContinuityServer.Voice.Sessions;
using ContinuityServer.Voice.Transport;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<VoiceOptions>(builder.Configuration.GetSection("Voice"));

builder.Services.AddSingleton<IVoiceSessionStore, InMemoryVoiceSessionStore>();
builder.Services.AddSingleton<IVoiceRouter, VoiceRouter>();

// Transport uses configured port:
builder.Services.AddSingleton<IVoiceTransport>(sp =>
{
    var opt = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<VoiceOptions>>().Value;
    return new UdpVoiceTransport(opt.UdpPort);
});

builder.Services.AddHostedService<Worker>();

await builder.Build().RunAsync();