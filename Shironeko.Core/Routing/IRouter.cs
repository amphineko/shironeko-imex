using Orleans;
using Shironeko.Core.Data;

namespace Shironeko.Core.Routing;

public interface IRouter : IGrainWithGuidKey
{
    Task Route(Event @event);
}