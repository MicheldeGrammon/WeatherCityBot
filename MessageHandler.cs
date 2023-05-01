using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PogodaCityBot
{
    internal class MessageHandler : IMessageHandler
    {
        private readonly IRequestWeather requestWeather;

        public MessageHandler(IRequestWeather requestWeather)
        {
            this.requestWeather = requestWeather;
        }

        public string GetAnswer(string userMessage, string chatId)
        {
            switch (userMessage)
            {
                case "/start":
                    return GetStartMessage();
                case "/list":
                    return GetListCity();
                case "/help":
                    return GetHelpInfo();
                case "/MyNotifications":
                    return GetMyNotifications(chatId);
                case "/ClearNotifications":
                    return ClearNotification(chatId);
                default:
                    if (TryGetId(userMessage, out var id))
                    {
                        return requestWeather.GetWeather(id);
                    }
                    else if (TryGetId(userMessage.Split('-')[0], out id))
                    {
                        return CreateNotification(chatId, userMessage);
                    }
                    return GetDefaultMessage(userMessage);
            }
        }

        private string GetStartMessage() => "Hello my name PogodaCityBot uses command /help";

        private string GetDefaultMessage(string userMessage) => $"Unknown name: \"{userMessage}\", check city name /list;";

        private string GetListCity()
        {
            var listCities = new StringBuilder();
            List<string> list;
            var count = 1;
            using (CitiesContext db = new CitiesContext())
            {
                list = db.CityIds.OrderBy(x => x.Name)
                                 .Select(x => x.Name)
                                 .ToList();

            }

            foreach (var item in list)
            {
                listCities.Append(count);
                listCities.Append(". ");
                listCities.AppendLine(item);
                count++;
            }

            return listCities.ToString();
        }

        private bool TryGetId(string userMessage, out string id)
        {
            id = "Invalid ID";
            userMessage = userMessage.ToUpper();

            using (CitiesContext db = new CitiesContext())
            {
                if (db.CityIds.Any(x => x.Name == userMessage))
                {
                    id = db.CityIds.Where(x => x.Name == userMessage)
                                   .Select(x => x.Id)
                                   .First();
                }
            }

            return id != "Invalid ID";
        }

        private string GetHelpInfo()
        {
            var helpInfo = new StringBuilder();
            helpInfo.AppendLine("My commands:");
            helpInfo.AppendLine("/start - initial greeting");
            helpInfo.AppendLine("/list - show a list of all cities in the database");
            helpInfo.AppendLine("\"/MyNotifications\" - show you notifications");
            helpInfo.AppendLine("\"/ClearNotifications\" - remove all notifications");
            helpInfo.AppendLine("syntax:\"NameCity-9:45\" - to create a daily notification");
            helpInfo.AppendLine("syntax:\"Name\" - show the weather in the city \"City Name\"");

            return helpInfo.ToString();
        }

        private string GetMyNotifications(string chatId)
        {
            using (NotificationsDbContext db = new NotificationsDbContext())
            {
                var notification = db.Notifications.Where(x => x.ChatId == chatId)
                                                   .Select(x => x)
                                                   .ToList();

                if (notification.Count() > 0)
                {
                    var sb = new StringBuilder();
                    foreach (var item in notification)
                    {
                        sb.AppendLine($"City = {item.NameCity}, Time = {item.Time}");
                    }

                    return sb.ToString();
                }

                return "you don't have notifications";
            }
        }

        private string CreateNotification(string chatId, string userMessage)
        {
            var input = userMessage.Split('-');


            string nameCity = input[0];
            string time = input[1];
            var inputTime = time.Split(':');

            if (inputTime.Length != 2)
            {
                return "wrong time";
            }

            var parseHourse = int.TryParse(inputTime[0], out var hours);
            var parseMinutes = int.TryParse(inputTime[1], out var minutes);

            if (!parseHourse || hours > 23 || !parseMinutes || minutes > 59)
            {
                return "wrong time";
            }

            using (NotificationsDbContext db = new NotificationsDbContext())
            {
                var done = 1;
                if (DateTime.Now.Hour <= hours && DateTime.Now.Minute < minutes)
                {
                    done = 0;
                }
                Notification notification = new Notification(chatId, nameCity, time, done);
                db.Add(notification);
                db.SaveChanges();
            }

            return "notification created";
        }

        private string ClearNotification(string chatId)
        {
            using (NotificationsDbContext db = new NotificationsDbContext())
            {
                var notification = db.Notifications.Where(x => x.ChatId == chatId)
                                                   .Select(x => x)
                                                   .ToList();

                foreach (var item in notification)
                {
                    db.Notifications.Remove(item);
                }
                db.SaveChanges();
            }

            return "all notifications removed";
        }
    }
}
