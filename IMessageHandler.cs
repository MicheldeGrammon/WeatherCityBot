namespace WeatherCityBot
{
    internal interface IMessageHandler
    {
        Task<string> GetAnswerAsync(string userMessage, string chatId);
    }
}