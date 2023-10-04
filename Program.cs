using WeatherCityBot;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace WeatherCityBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //test GitHub
            ITelegramBotClient bot = new TelegramBotClient("token");
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            using var cts = new CancellationTokenSource();
            var requestWeather = new RequestWeather();
            var messageHandler = new MessageHandler(requestWeather);
            var updateHandler = new UpdateHandler(messageHandler);
            var errorHeadnler = new ErrorHandler();
            var notificationHandler = new NotificationHandler(bot, messageHandler);
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            bot.StartReceiving(
                updateHandler.HandleUpdateAsync,
                errorHeadnler.HandleErrorAsync,
                receiverOptions,
                cts.Token
            );
            notificationHandler.GetNotificationAsync();

            Console.ReadLine();
        }
    }
}