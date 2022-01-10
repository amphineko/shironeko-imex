using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Shironeko.Telegram.Launcher;

var host = new HostBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        builder
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .Build();
    })
    .ConfigureLogging(builder =>
    {
        builder.AddConsole();
        builder.SetMinimumLevel(LogLevel.Debug);
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton(sp => sp.GetRequiredService<IConfiguration>().Get<Configuration>());

        services.AddSingleton<ClusterClientHostedService>();
        services.AddHostedService<ClusterClientHostedService>();
        services.AddSingleton(sp => sp.GetRequiredService<ClusterClientHostedService>().Client);
        services.AddSingleton<IGrainFactory>(sp => sp.GetRequiredService<ClusterClientHostedService>().Client);

        services.AddHostedService<TelegramBotClientHostedService>();
    })
    .Build();

await host.RunAsync();