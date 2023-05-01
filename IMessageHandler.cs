namespace PogodaCityBot
{
    internal interface IMessageHandler
    {
        string GetAnswer(string userMessage, string chatId);
    }
}