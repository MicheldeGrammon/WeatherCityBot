using Telegram.Bot;
using Telegram.Bot.Types;

namespace WeatherCityBot
{
    internal class ErrorHandler
    {
        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}
