using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using WeatherForecastRemote.Core.Interfaces.Data;
using WeatherForecastRemote.Core.Models;
using WeatherForecastRemote.Infrastructure.Data.Config;

namespace WeatherForecastRemote.Infrastructure.Data
{
    public class WeatherClient : IWeatherClient
    {
        private readonly HttpClient httpClient;
        private readonly ServiceSettings settings;

        public WeatherClient(HttpClient httpClient, IOptions<ServiceSettings> options)
        {
            this.httpClient = httpClient;
            this.settings = options.Value;
        }

        public async Task<Forecast> GetCurrentWeatherAsync(string city)
        {
            var forecast = await httpClient.GetFromJsonAsync<Forecast>($"https://{settings.OpenWeatherHost}/data/2.5/weather?q={city}&appid={settings.ApiKey}");
            return forecast;
        }
    }
}
