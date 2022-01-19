using Orleans;
using Shironeko.Core.Data;
using Shironeko.Core.Extensions;

namespace Shironeko.Core.Routing;

public class EventContextGrain : Grain, IEventContext
{
    private readonly IEnumerator<IEventHandler<Event>> _handlers;
    private Event? _event;

    public EventContextGrain(IEventHandlerActivator activator)
    {
        _handlers = activator.EventHandlers.GetEnumerator();
    }

    public Task Configure(Event @event)
    {
        _event = @event;
        return Task.CompletedTask;
    }

    public async Task InvokeNext()
    {
        if (_event is null) throw new InvalidOperationException("Event is not configured");

        if (_handlers.MoveNext()) await _handlers.Current.Handle(_event, this);
    }
}