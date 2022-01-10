using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shironeko.Launcher;

var host = new HostBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddJsonFile($"{typeof(Program).Assembly.GetName().Name}.appsettings.json", false);
    })
    .ConfigureLogging(builder =>
    {
        builder.AddConsole();
        builder.SetMinimumLevel(LogLevel.Debug);
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton(sp => sp.GetRequiredService<IConfiguration>().Get<Configuration>());

        services.AddSingleton<SiloHostService>();
        services.AddHostedService<SiloHostService>();
    })
    .Build();

await host.RunAsync();