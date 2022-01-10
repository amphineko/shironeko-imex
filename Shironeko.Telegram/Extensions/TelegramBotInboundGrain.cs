using Orleans;
using Shironeko.Core.Data.Messages;
using Shironeko.Core.Routing;

namespace Shironeko.Telegram.Extensions;

public class TelegramBotInboundGrain : Grain, ITelegramBotInbound
{
    private IRouter _router = null!;

    public async Task ReceiveMessage(Message message)
    {
        await _router.Route(message);
    }

    public override Task OnActivateAsync()
    {
        _router = GrainFactory.GetGrain<IRouter>(Guid.Empty);
        return Task.CompletedTask;
    }
}