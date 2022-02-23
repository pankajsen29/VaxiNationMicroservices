namespace WeatherForecastRemote.Core.Models
{
    public record Weather(string description);
    public record Main(decimal temp);
    public record Forecast(Weather[] weather, Main main, long dt);
}
