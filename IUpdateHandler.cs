using Telegram.Bot;
using Telegram.Bot.Types;

namespace WeatherCityBot
{
    internal interface IUpdateHandler
    {
        Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    }
}