using Telegram.Bot;
using Telegram.Bot.Types;

namespace PogodaCityBot
{
    internal interface IUpdateHandler
    {
        Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    }
}