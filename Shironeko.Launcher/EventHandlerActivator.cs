using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shironeko.Core.Data;
using Shironeko.Core.Extensions;
using Shironeko.Core.Routing;

namespace Shironeko.Launcher;

public class EventHandlerActivator : IEventHandlerActivator
{
    public EventHandlerActivator(IConfiguration configurationRoot, IExtensionRegistry registry,
        IServiceProvider services)
    {
        var children = configurationRoot.GetSection("EventHandlers").GetChildren();
        EventHandlers = children.Select((child, index) =>
        {
            var section = child.Get<EventHandlerConfigurationSection>();

            // resolve handler type

            Type? type = null;
            if (section.Guid is { } guid) type = registry.GetByGuid(guid);
            if (section.Name is not null) type = registry.GetByName(section.Name);

            if (type is null)
                throw new InvalidOperationException($"Cannot find type for event handler at index {index}");

            if (!type.IsAssignableTo(typeof(IEventHandler<Event>)))
                throw new InvalidOperationException($"{type.Name} is not assignable to {nameof(IEventHandler<Event>)}");

            // retrieve actual configuration type from handler attribute

            var attribute = type.GetCustomAttribute<EventHandlerAttribute>();
            if (attribute is null)
                throw new InvalidOperationException($"{nameof(EventHandlerAttribute)} is missing on {type.FullName}");
            var configType = attribute.ConfigurationType;

            // activate

            var configuration = child.GetSection("Configuration").Get(configType);
            if (configuration is not null)
                return (IEventHandler<Event>) ActivatorUtilities.CreateInstance(services, type, configuration);

            return (IEventHandler<Event>) ActivatorUtilities.CreateInstance(services, type);
        }).ToList();
    }

    public IReadOnlyList<IEventHandler<Event>> EventHandlers { get; }
}