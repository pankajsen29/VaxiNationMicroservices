using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text.Json;
using VaccineInfo.Infrastructure.Data.Config;

namespace VaccineInfo.Api.Extensions
{
    /// <summary>
    /// Adds HealthCheck service
    /// </summary>
    public static class HealthCheckConfiguration
    {
        public static IServiceCollection AddHealthCheckService(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var mongoDbsettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

            services.AddHealthChecks().AddMongoDb(
                mongoDbsettings.ConnectionString,
                name: "mongodb",
                timeout: TimeSpan.FromSeconds(3),
                tags: new[] { "ready" });

            return services;
        }

        public static IApplicationBuilder ConfigureHealthCheckEndpoint(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("api/health/ready", new HealthCheckOptions
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                    ResponseWriter = async (context, report) =>
                    {
                        var result = JsonSerializer.Serialize(
                                new
                                {
                                    status = report.Status.ToString(),
                                    checks = report.Entries.Select(entry => new
                                    {
                                        name = entry.Key,
                                        status = entry.Value.Status.ToString(),
                                        exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                                        duration = entry.Value.Duration.ToString()
                                    })
                                }
                            );
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });

                endpoints.MapHealthChecks("api/health/live", new HealthCheckOptions
                {
                    Predicate = (_) => false
                });
            });

            return app;
        }
    }
}
