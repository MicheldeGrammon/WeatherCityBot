using Telegram.Bot;

namespace WeatherCityBot
{
    internal interface IErrorHandler
    {
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
    }
}