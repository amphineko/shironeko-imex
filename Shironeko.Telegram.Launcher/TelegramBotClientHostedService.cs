using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Shironeko.Core.Data.Messages;
using Shironeko.Telegram.Extensions;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Message = Shironeko.Core.Data.Messages.Message;

namespace Shironeko.Telegram.Launcher;

public class TelegramBotClientHostedService : IHostedService
{
    private readonly TelegramBotClient _client;
    private readonly Task _clusterClientConnectTask;

    private readonly CancellationTokenSource _cts = new();
    private readonly IGrainFactory _grainFactory;
    private readonly ILogger<ClusterClientHostedService> _logger;

    private ITelegramBotInbound _inbound = null!;

    private User _me = null!;

    public TelegramBotClientHostedService(Configuration configuration, ClusterClientHostedService clusterClientService,
        ILogger<ClusterClientHostedService> logger)
    {
        _logger = logger;
        _clusterClientConnectTask = clusterClientService.ConnectTask;
        _grainFactory = clusterClientService.Client;
        _client = new TelegramBotClient(configuration.Token);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _clusterClientConnectTask;
        _inbound = _grainFactory.GetGrain<ITelegramBotInbound>(Guid.NewGuid());

        _me = await _client.GetMeAsync(_cts.Token);

        _client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, new ReceiverOptions
        {
            AllowedUpdates = new[]
            {
                UpdateType.Message
            }
        }, _cts.Token);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts.Cancel();
        return Task.CompletedTask;
    }

    private Task HandleErrorAsync(ITelegramBotClient _, Exception error, CancellationToken cancellationToken)
    {
        _logger.LogError(error, "TelegramBotClient error: {Error}", error.ToString());
        return Task.CompletedTask;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        if (update.Message?.Type != MessageType.Text) return;

        var part = new TextMessagePart(update.Message.Text ?? "");
        var message = new Message
        {
            Contents = new[] {part},
            Context = new Uri($"telegram://{update.Message.Chat.Id}/"),
            Recipients = update.Message.Chat.Type switch
            {
                // TODO: Add support for targeted reply to message
                _ => Array.Empty<Uri>()
            },
            Sender = update.Message.From is not null ? new Uri($"telegram://{update.Message.From.Id}/") : null
        };

        await _inbound.ReceiveMessage(message);
    }
}