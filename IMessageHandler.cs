namespace WeatherCityBot
{
    internal interface IMessageHandler
    {
        string GetAnswer(string userMessage, string chatId);
    }
}