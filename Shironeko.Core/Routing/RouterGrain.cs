using Orleans;
using Shironeko.Core.Data;

namespace Shironeko.Core.Routing;

public class RouterGrain : Grain, IRouter
{
    public async Task Route(Event @event)
    {
        var context = GrainFactory.GetGrain<IEventContext>(Guid.NewGuid());
        await context.Configure(@event);
        await context.InvokeNext();
    }
}