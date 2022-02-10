using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VaccineInfo.Api.Extensions
{
    /// <summary>
    /// Swagger configuration including API versioning
    /// </summary>
    public static class SwaggerConfiguration
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
            services.AddVersionedApiExplorer(config =>
            {
                config.GroupNameFormat = "'v'VVV"; //Major, optional minor version, and status
                config.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen().AddSwaggerGenNewtonsoftSupport();
            //option configuration to create the JSON documents what swagger uses to create the UI
            services.ConfigureOptions<ConfigureSwaggerOptions>();

            return services;
        }

        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                //to create the endpoints for swagger for each version, here is how the middleware is configured 
                app.UseSwaggerUI(options =>
                {
                    var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.ApiVersion.ToString());
                    }
                });
            }
            return app;
        }
    }

    /// <summary>
    /// configuration option class which is responsible to create the JSON documents what swagger uses to create the UI
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            this._provider = provider;
        }
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "VaccineInfo.Api",
                Version = description.ApiVersion.ToString(),
            };

            //additional info if an API is marked as deprecated
            if (description.IsDeprecated)
            {
                info.Description = "[Deprecated API version]";
            }
            return info;
        }
    }
}
