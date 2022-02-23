using WeatherForecastRemote.Core.Interfaces.Data;
using WeatherForecastRemote.Core.Interfaces.Services;
using WeatherForecastRemote.Core.Models;

namespace WeatherForecastRemote.Core.Services
{
    public class WeatherService:IWeatherService
    {
        private readonly IWeatherClient _weatherClient;

        public WeatherService(IWeatherClient weatherClient)
        {
            this._weatherClient = weatherClient;
        }

        public async Task<Forecast> GetCurrentWeatherAsync(string city)
        {
            return await _weatherClient.GetCurrentWeatherAsync(city);
        }      
    }
}
