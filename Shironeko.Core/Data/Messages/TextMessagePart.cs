namespace Shironeko.Core.Data.Messages;

public class TextMessagePart : MessagePart
{
    public TextMessagePart(string text)
    {
        Text = text;
        Type = MessagePartType.Text;
    }

    public string Text { get; }

    public override string GetText()
    {
        return Text;
    }
}