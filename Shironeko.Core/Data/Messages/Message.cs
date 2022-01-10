namespace Shironeko.Core.Data.Messages;

public class Message : Event
{
    public MessagePart[] Contents { get; init; } = Array.Empty<MessagePart>();

    public Uri[] Recipients { get; init; } = Array.Empty<Uri>();

    /**
     * Convert the message to its string representation.
     */
    public string GetText()
    {
        return string.Join(" ", Contents.Select(x => x.GetText()));
    }
}