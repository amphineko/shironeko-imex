namespace Shironeko.Core.Data.Messages;

public class Message : Event
{
    public static readonly Message Empty = new();

    /**
     * Contents of a multimedia message.
     */
    public MessagePart[] Contents { get; set; } = Array.Empty<MessagePart>();

    /**
     * Targeted recipient of the message.
     *
     * Example of usage: a message quoted and sending to a specific user in a group.
     */
    public Uri[] Recipients { get; set; } = Array.Empty<Uri>();

    /**
     * Convert the message to its string representation.
     */
    public string GetText()
    {
        return string.Join(" ", Contents.Select(x => x.GetText()));
    }
}