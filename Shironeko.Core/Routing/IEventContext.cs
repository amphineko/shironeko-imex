using Orleans;
using Shironeko.Core.Data;

namespace Shironeko.Core.Routing;

public interface IEventContext : IGrainWithGuidKey
{
    Task Configure(Event @event);

    Task InvokeNext();
}