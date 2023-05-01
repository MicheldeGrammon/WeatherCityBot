using System.Text.Json;


namespace PogodaCityBot
{
    internal class RequestWeather : IRequestWeather
    {
        private readonly HttpClient httpClient = new HttpClient();
        public string GetWeather(string id)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?id={id}&units=metric&appid=e2b73f95db062d5a0de631582da9b2f5&lang=ru";

            string response;

            using (HttpClient httpClient = new HttpClient())
            {
                var responseMessage = httpClient.GetAsync(url).Result;
                response = responseMessage.Content.ReadAsStringAsync().Result;
            }

            WeatherResponse weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(response);
            string city = weatherResponse.name;
            double temp = weatherResponse.main.temp;
            string weather = weatherResponse.weather[0].description;

            return $"{city}{Environment.NewLine}Температура: {temp}{Environment.NewLine}{weather}";
        }
    }
}
