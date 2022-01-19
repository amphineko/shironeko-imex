using Microsoft.Extensions.Logging;
using Shironeko.Core.Data;
using Shironeko.Core.Data.Messages;
using Shironeko.Core.Extensions;

namespace Shironeko.Core.Routing;

[EventHandler(nameof(BlackholeHandler), typeof(BlackholeHandlerConfiguration))]
public class BlackholeHandler : IEventHandler<Event>
{
    private static readonly BlackholeHandlerConfiguration DefaultConfiguration = new()
    {
        LogMessages = false
    };

    private readonly ILogger<BlackholeHandler> _logger;

    private readonly bool _shouldLogMessages;

    public BlackholeHandler(ILogger<BlackholeHandler> logger) : this(DefaultConfiguration, logger)
    {
    }

    public BlackholeHandler(BlackholeHandlerConfiguration configuration, ILogger<BlackholeHandler> logger)
    {
        _logger = logger;
        _shouldLogMessages = configuration.LogMessages;
    }

    public Task Handle(Event @event, IEventContext context)
    {
        if (_shouldLogMessages && @event is Message message)
            _logger.LogInformation("{Sender}: {Message}", message.Sender, message.GetText());

        return Task.CompletedTask;
    }
}