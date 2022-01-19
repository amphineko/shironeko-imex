using Shironeko.Core.Data;
using Shironeko.Core.Routing;

namespace Shironeko.Core.Extensions;

public interface IEventHandlerActivator
{
    IReadOnlyList<IEventHandler<Event>> EventHandlers { get; }
}