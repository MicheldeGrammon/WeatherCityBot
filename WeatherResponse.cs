namespace WeatherCityBot
{
    public class WeatherResponse
    {
        public WeatherTemp main { get; set; }
        public List<WeatherDescription> weather { get; set; }
        public string name { get; set; }
    }
}