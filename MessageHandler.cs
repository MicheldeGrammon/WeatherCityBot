using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCityBot
{
    internal class MessageHandler : IMessageHandler
    {
        private readonly IRequestWeather requestWeather;

        public MessageHandler(IRequestWeather requestWeather)
        {
            this.requestWeather = requestWeather;
        }

        public async Task<string> GetAnswerAsync(string userMessage, string chatId)
        {
            switch (userMessage)
            {
                case "/start":
                    return GetStartMessage();
                case "/list":
                    return await GetListCityAsync();
                case "/help":
                    return GetHelpInfo();
                case "/MyNotifications":
                    return await GetMyNotificationsAsync(chatId);
                case "/ClearNotifications":
                    return await ClearNotificationAsync(chatId);
                default:
                    var id = await GetIdAsync(userMessage);
                    if (id != "Invalid ID")
                    {
                        return await requestWeather.GetWeatherAsync(id);
                    }
                    else if (await GetIdAsync(userMessage.Split('-')[0]) != "Invalid ID")
                    {
                        return await CreateNotificationAsync(chatId, userMessage);
                    }
                    return GetDefaultMessage(userMessage);
            }
        }

        private string GetStartMessage() => "Hello my name PogodaCityBot uses command /help";

        private string GetDefaultMessage(string userMessage) => $"Unknown name: \"{userMessage}\", check city name /list;";

        private async Task<string> GetListCityAsync()
        {
            var listCities = new StringBuilder();
            List<string> list;
            var count = 1;

            using (CitiesContext db = new CitiesContext())
            {
                list = await db.CityIds.OrderBy(x => x.Name)
                                       .Select(x => x.Name)
                                       .ToListAsync();
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

        private async Task<string> GetIdAsync(string userMessage)
        {
            var id = "Invalid ID";
            userMessage = userMessage.ToUpper();

            using (CitiesContext db = new CitiesContext())
            {
                if (db.CityIds.Any(x => x.Name == userMessage))
                {
                    id = await db.CityIds.Where(x => x.Name == userMessage)
                                         .Select(x => x.Id)
                                         .FirstAsync();
                }
            }

            return id;
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

        private async Task<string> GetMyNotificationsAsync(string chatId)
        {
            using (NotificationsDbContext db = new NotificationsDbContext())
            {
                var notification = await db.Notifications.Where(x => x.ChatId == chatId)
                                                         .Select(x => x)
                                                         .ToListAsync();

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

        private async Task<string> CreateNotificationAsync(string chatId, string userMessage)
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
                await db.SaveChangesAsync();
            }

            return "notification created";
        }

        private async Task<string> ClearNotificationAsync(string chatId)
        {
            using (NotificationsDbContext db = new NotificationsDbContext())
            {
                var notification = db.Notifications.Where(x => x.ChatId == chatId)
                                                   .Select(x => x);

                foreach (var item in notification)
                {
                    db.Notifications.Remove(item);
                }
                await db.SaveChangesAsync();
            }

            return "all notifications removed";
        }
    }
}
