using Telegram.Bot;
using Telegram.Bot.Types;

namespace WeatherCityBot
{
    internal class UpdateHandler : IUpdateHandler
    {
        private readonly IMessageHandler messageHandler;

        public UpdateHandler(IMessageHandler messageHandler)
        {
            this.messageHandler = messageHandler;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var date = update.Message!.Date.AddHours(5); //message time + time zone
                        var dateMessage = DateTime.Now - date;

            if (dateMessage.Hours < 1)
            {
                var message = update.Message;
                var chatId = message.Chat.Id;
                Console.WriteLine($"Received a '{message.Text}' message in chat {chatId}.");

                if (message.Text == null) message.Text = "/start";
                {
                    await botClient.SendTextMessageAsync(message.Chat, await messageHandler.GetAnswerAsync(message.Text, chatId.ToString()));
                }
            }
        }
    }
}
