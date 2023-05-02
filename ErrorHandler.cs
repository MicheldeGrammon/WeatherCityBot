using Telegram.Bot;

namespace WeatherCityBot
{
    internal class ErrorHandler : IErrorHandler
    {
        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
        
    }
}
