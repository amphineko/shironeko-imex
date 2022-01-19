using System.Reflection;
using Microsoft.Extensions.Logging;
using Shironeko.Core.Extensions;

namespace Shironeko.Launcher;

public class ExtensionRegistry : IExtensionRegistry
{
    private readonly IDictionary<Guid, Type?> _extensionsByGuid = new Dictionary<Guid, Type?>();

    private readonly IDictionary<string, Type?> _extensionsByName = new Dictionary<string, Type?>();

    public ExtensionRegistry(Configuration configuration, ILogger<ExtensionRegistry> logger)
    {
        var assemblies = configuration.Extensions.Select(filename => Assembly.LoadFrom(Path.GetFullPath(filename)));
        IEnumerable<Type?> types = assemblies.SelectMany(assembly => assembly.GetTypes());

        foreach (var type in types)
        {
            if (type.GetCustomAttribute<EventHandlerAttribute>() is not { } attribute) continue;

            if (attribute.Guid is { } guid)
            {
                logger.LogDebug("Registering extension {FullName} with GUID {Guid}", type.FullName, guid);
                _extensionsByGuid[guid] = type;
            }

            if (attribute.Name is not null)
            {
                logger.LogDebug("Registering extension {FullName} with name {Name}", type.FullName, attribute.Name);
                _extensionsByName[attribute.Name] = type;
            }
        }
    }

    public Type? GetByGuid(Guid guid)
    {
        return _extensionsByGuid[guid];
    }

    public Type? GetByName(string name)
    {
        return _extensionsByName[name];
    }
}