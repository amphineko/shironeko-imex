using Shironeko.Core.Extensions;

namespace Shironeko.Launcher;

public class Configuration
{
    public string ClusterId { get; set; } = "";

    public string[] Extensions { get; set; } = Array.Empty<string>();

    public EventHandlerConfigurationSection[] Handlers { get; set; } = Array.Empty<EventHandlerConfigurationSection>();

    public string ServiceId { get; set; } = "";
}