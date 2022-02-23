using WeatherForecastRemote.Core.Models;

namespace WeatherForecastRemote.Core.Interfaces.Services
{
    public interface IWeatherService
    {
        Task<Forecast> GetCurrentWeatherAsync(string city);
    }
}