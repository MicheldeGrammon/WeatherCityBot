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
            var date = update.Message!.Date;
            var dateMessage = DateTime.Now - date;

            if (dateMessage.Seconds < 2)
            {
                var message = update.Message;
                var chatId = message.Chat.Id;
                Console.WriteLine($"Received a '{message.Text}' message in chat {chatId}.");

                if (message.Text == null) message.Text = "/start";
                {
                    await botClient.SendTextMessageAsync(message.Chat, messageHandler.GetAnswer(message.Text, chatId.ToString()));
                }
            }
        }
    }
}
