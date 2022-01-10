namespace Shironeko.Core.Data.Messages;

public abstract class MessagePart
{
    protected MessagePart(MessagePartType type)
    {
        Type = type;
    }

    public MessagePartType Type { get; }

    public abstract string GetText();
}