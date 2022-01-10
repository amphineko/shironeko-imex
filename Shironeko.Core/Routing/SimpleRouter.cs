using Microsoft.Extensions.Logging;
using Orleans;
using Shironeko.Core.Data;
using Shironeko.Core.Data.Messages;

namespace Shironeko.Core.Routing;

public class SimpleRouter : Grain, IRouter
{
    private readonly ILogger<SimpleRouter> _logger;

    public SimpleRouter(ILogger<SimpleRouter> logger)
    {
        _logger = logger;
    }

    public Task Route(Event @event)
    {
        // TODO: implement routing logic

        if (@event.Sender is not null && @event is Message message)
            _logger.LogInformation("{Sender}: {Text}", @event.Sender, message.GetText());

        return Task.CompletedTask;
    }
}