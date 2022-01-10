using Orleans;
using Shironeko.Core.Data.Messages;

namespace Shironeko.Telegram.Extensions;

public interface ITelegramBotInbound : IGrainWithGuidKey
{
    Task ReceiveMessage(Message message);
}