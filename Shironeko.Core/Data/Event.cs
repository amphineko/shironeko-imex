namespace Shironeko.Core.Data;

public abstract class Event
{
    private static readonly Uri Null = new("null:///");

    /**
     * The context of the event (e.g. a group or a private chat).
     */
    public Uri Context { get; set; } = Null;

    /**
     * The user who initiated the event.
     */
    public Uri? Sender { get; set; } = Null;
}