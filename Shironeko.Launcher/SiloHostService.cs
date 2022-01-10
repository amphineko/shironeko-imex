using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
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