namespace Shironeko.Core.Data.Messages;

public class TextMessagePart : MessagePart
{
    public TextMessagePart(string text) : base(MessagePartType.Text)
    {
        Text = text;
    }

    public string Text { get; }

    public override string GetText()
    {
        return Text;
    }
}