using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("WEATHER_API_KEY") ?? throw new Exception("Weather API key not found");
        }

        public async Task<WeatherApiResponse?> GetWeatherAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City is required", nameof(city));

            var url = $"https://api.weatherapi.com/v1/forecast.json?key={_apiKey}&q={city}&days=3";
            var response = await _httpClient.GetAsync(url);

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Weather API error: {response.StatusCode}, {body}");

            return JsonSerializer.Deserialize<WeatherApiResponse>(body);
        }
    }

    public class WeatherApiResponse
    {
        public Location location { get; set; }
        public Current current { get; set; }
        public Forecast forecast { get; set; }

        public class Location
        {
            public string name { get; set; }
            public string country { get; set; }
            public string localtime { get; set; }
        }

        public class Current
        {
            public float temp_c { get; set; }
            public float wind_kph { get; set; }
            public float humidity { get; set; }
            public Condition condition { get; set; }
            public float feelslike_c { get; set; }
        }

        public class Forecast
        {
            public List<ForecastDay> forecastday { get; set; }
        }

        public class ForecastDay
        {
            public string date { get; set; }
            public Day day { get; set; }
        }

        public class Day
        {
            public float maxtemp_c { get; set; }
            public float mintemp_c { get; set; }
            public float avgtemp_c { get; set; }
            public Condition condition { get; set; }
        }

        public class Condition
        {
            public string text { get; set; }
            public string icon { get; set; }
        }
    }
}
