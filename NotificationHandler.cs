using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WeatherCityBot
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

        public async Task GetNotificationAsync()
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
                        await db.SaveChangesAsync();
                    }

                    var listNotifications = db.Notifications.Where(x => x.Done == 0)
                                                            .Select(x => x);

                    if (listNotifications.Count()>0) 
                    {
                        foreach (var notification in listNotifications)
                        {
                                var inputTime = notification.Time.Split(':');
                                if (int.Parse(inputTime[0]) <= hours && int.Parse(inputTime[1]) < minutes)
                                {
                                    await botClient.SendTextMessageAsync(notification.ChatId, await messageHandler.GetAnswerAsync(notification.NameCity, notification.ChatId));
                                    notification.Done = 1;
                                }
                                await db.SaveChangesAsync();
                        }
                    }                    
                }
            }
        }
    }
}
