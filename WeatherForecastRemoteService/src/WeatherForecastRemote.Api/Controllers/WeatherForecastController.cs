using Microsoft.AspNetCore.Mvc;
using WeatherForecastRemote.Api.Dtos;
using WeatherForecastRemote.Core.Interfaces.Services;

namespace WeatherForecastRemote.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(IWeatherService weatherService)
        {
            this._weatherService = weatherService;
        }

        [HttpGet]
        [Route("{city}")]
        public async Task<WeatherDto> Get(string city)
        {
            var forecast = await _weatherService.GetCurrentWeatherAsync(city);

            return new WeatherDto
            {
                Summary = forecast.weather[0].description,
                TemperatureC = (int)forecast.main.temp,
                Date = DateTimeOffset.FromUnixTimeSeconds(forecast.dt).DateTime
            };
        }
    }
}