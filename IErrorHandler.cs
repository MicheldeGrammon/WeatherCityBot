using Telegram.Bot;

namespace PogodaCityBot
{
    internal interface IErrorHandler
    {
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
    }
}