using WeatherForecastRemote.Core.Models;

namespace WeatherForecastRemote.Core.Interfaces.Data
{
    public interface IWeatherClient
    {
        Task<Forecast> GetCurrentWeatherAsync(string city);
    }
}