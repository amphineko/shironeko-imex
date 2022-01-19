namespace Shironeko.Core.Data.Messages;

public abstract class MessagePart
{
    public MessagePartType Type { get; init; }

    public abstract string GetText();
}