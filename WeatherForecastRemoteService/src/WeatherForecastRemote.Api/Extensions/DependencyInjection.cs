using Polly;
using WeatherForecastRemote.Core.Interfaces.Data;
using WeatherForecastRemote.Core.Interfaces.Services;
using WeatherForecastRemote.Core.Services;
using WeatherForecastRemote.Infrastructure.Data;
using WeatherForecastRemote.Infrastructure.Data.Config;

namespace WeatherForecastRemote.Api.Extensions
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Extension method to inject all the dependencies in a separate place, 
        /// this helps when the dependencies grow or the application grows.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            //remote service configurations are read and will be populated in an instance of ServiceSettings and also got registered in the dependency engine
            services.Configure<ServiceSettings>(configuration.GetSection(nameof(ServiceSettings)));            

            //Register remote weather service classes 
            services.AddScoped<IWeatherService, WeatherService>();
            services.AddScoped<IWeatherClient, WeatherClient>();

            //registered HttpClient
            services.AddHttpClient<IWeatherClient, WeatherClient>()
                .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(3, TimeSpan.FromSeconds(10)));

            return services;
        }
    }
}
