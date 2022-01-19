using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Shironeko.Core.Extensions;
using Shironeko.Core.Routing;

namespace Shironeko.Launcher;

public class SiloHostService : IHostedService
{
    private readonly ISiloHost _host;

    public SiloHostService(Configuration config, ILogger<SiloHostService> logger)
    {
        _host = new SiloHostBuilder()
            .Configure<ClusterOptions>(options =>
            {
                logger.LogDebug(
                    "Configuring cluster: ClusterId={ClusterId}, ServiceId={ServiceId}",
                    config.ClusterId, config.ServiceId
                );

                options.ClusterId = config.ClusterId;
                options.ServiceId = config.ServiceId;
            })
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddJsonFile($"{typeof(Program).Assembly.GetName().Name}.json", false);
            })
            .ConfigureApplicationParts(partManager =>
            {
                partManager.AddApplicationPart(typeof(IRouter).Assembly).WithReferences();

                foreach (var filename in config.Extensions)
                {
                    var filePath = Path.GetFullPath(filename);
                    logger.LogDebug("Attempting to load extension from {filePath}", filePath);

                    var assembly = Assembly.LoadFile(filePath);
                    partManager.AddApplicationPart(assembly).WithReferences();
                }
            })
            .ConfigureLogging(builder => { builder.AddConsole(); })
            .ConfigureServices(services =>
            {
                services.AddSingleton(sp => sp.GetRequiredService<IConfiguration>().Get<Configuration>());

                services.AddSingleton<ExtensionRegistry>();
                services.AddSingleton<IExtensionRegistry>(sp => sp.GetRequiredService<ExtensionRegistry>());

                services.AddSingleton<EventHandlerActivator>();
                services.AddSingleton<IEventHandlerActivator>(sp => sp.GetRequiredService<EventHandlerActivator>());
            })
            .UseLocalhostClustering()
            .Build();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _host.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _host.StopAsync(cancellationToken);
    }
}