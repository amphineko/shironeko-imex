namespace Shironeko.Launcher;

public class Configuration
{
    public string ClusterId { get; set; } = "";

    public string[] Extensions { get; set; } = Array.Empty<string>();

    public string ServiceId { get; set; } = "";
}