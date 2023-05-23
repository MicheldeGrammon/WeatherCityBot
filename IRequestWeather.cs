namespace WeatherCityBot
{
    internal interface IRequestWeather
    {
        Task<string> GetWeatherAsync(string id);
    }
}