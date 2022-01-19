namespace Shironeko.Core.Extensions;

[AttributeUsage(AttributeTargets.Class)]
public class EventHandlerAttribute : Attribute
{
    public EventHandlerAttribute(Guid guid, Type? configurationClass = null)
    {
        ConfigurationType = configurationClass ?? typeof(object);
        Guid = guid;
    }

    public EventHandlerAttribute(string name, Type? configurationClass = null)
    {
        ConfigurationType = configurationClass ?? typeof(object);
        Name = name;
    }

    public EventHandlerAttribute(Guid guid, string name, Type? configurationClass = null)
    {
        ConfigurationType = configurationClass ?? typeof(object);
        Guid = guid;
        Name = name;
    }

    public Type ConfigurationType { get; }

    public Guid? Guid { get; }

    public string? Name { get; }
}