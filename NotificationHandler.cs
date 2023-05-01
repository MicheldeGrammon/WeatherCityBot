using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PogodaCityBot
{
    internal class NotificationHandler
    {
        private readonly ITelegramBotClient botClient;
        private readonly MessageHandler messageHandler;

        public NotificationHandler(ITelegramBotClient bot, MessageHandler messageHandler)
        {
            this.botClient = bot;
            this.messageHandler = messageHandler;
        }

        public async void GetNotification()
        {

            while (true)
            {
                await Task.Delay(60000);
                using (NotificationsDbContext db = new NotificationsDbContext())
                {
                    var hours = int.Parse(DateTime.Now.Hour.ToString());
                    var minutes = int.Parse(DateTime.Now.Minute.ToString());

                    if (DateTime.Now.Hour == 0 && DateTime.Now.Minute <= 1)
                    {
                        var resetDone = db.Notifications.ToList();

                        foreach (var item in resetDone)
                        {
                            item.Done = 0;
                        }
                        db.SaveChanges();
                    }

                    var list = db.Notifications.Where(x => x.Done == 0)
                                               .Select(x => x)
                                               .ToList();

                    foreach (var item in list)
                    {
                        var inputTime = item.Time.Split(':');
                        if (int.Parse(inputTime[0]) <= hours && int.Parse(inputTime[1]) < minutes)
                        {
                            await botClient.SendTextMessageAsync(item.ChatId, messageHandler.GetAnswer(item.NameCity, item.ChatId));
                            item.Done = 1;
                        }
                        db.SaveChanges();
                    }
                }
            }

        }
    }
}
