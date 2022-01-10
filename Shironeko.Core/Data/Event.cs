namespace Shironeko.Core.Data;

public class Event
{
    private static readonly Uri NullUri = new("null:///");

    /**
     * The context of the event (e.g. a group or a private chat).
     */
    public Uri Context { get; set; } = NullUri;

    /**
     * The user who initiated the event.
     */
    public Uri? Sender { get; set; } = NullUri;
}