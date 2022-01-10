using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Shironeko.Core.Routing;
using Shironeko.Telegram.Extensions;

namespace Shironeko.Telegram.Launcher;

public class ClusterClientHostedService : IHostedService
{
    private readonly ILogger<ClusterClientHostedService> _logger;
    public readonly Task ConnectTask;

    public ClusterClientHostedService(Configuration configuration, ILogger<ClusterClientHostedService> logger)
    {
        _logger = logger;

        // TODO: configurable cluster endpoints

        Client = new ClientBuilder()
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = configuration.ClusterId;
                options.ServiceId = configuration.ServiceId;
            })
            .ConfigureApplicationParts(parts =>
            {
                parts.AddApplicationPart(typeof(IRouter).Assembly).WithReferences();
                parts.AddApplicationPart(typeof(ITelegramBotInbound).Assembly).WithReferences();
            })
            .UseLocalhostClustering()
            .Build();

        ConnectTask = Client.Connect();
    }

    public IClusterClient Client { get; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await ConnectTask;
        _logger.LogInformation("Connected to cluster");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Disconnecting from cluster");
        await Client.Close();
        await Client.DisposeAsync();
        _logger.LogInformation("Disconnected from cluster");
    }
}