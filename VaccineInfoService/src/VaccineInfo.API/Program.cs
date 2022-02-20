using Serilog;
using VaccineInfo.Api.Extensions;
using VaccineInfo.Api.Mappings;
using VaccineInfo.Api.Middlewares;

try
{
    //Log.Information("Starting the web host..");
    var builder = WebApplication.CreateBuilder(args);

    //configure Serilog
    builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    {
        loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
    });

    // Add services to the container.
    ConfigureServices(builder.Services, builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    ConfigureMiddleware(app, app.Environment);
    ConfigureEndpoints(app);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Exiting the service..");
    Log.CloseAndFlush();
}


void ConfigureServices(IServiceCollection services, IConfiguration configuration) 
{
    //dependencies are injected in separate extension method
    services.ConfigureDependencyInjection(configuration);

    //SuppressAsyncSuffixInActionNames: to be able to find the action method without "Async" suffix at runtime,
    //at the case when that action is called from another action method 
    //AddNewtonsoftJson: this is for PATCH using JsonPatchDocument
    services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false).AddNewtonsoftJson();

    //Register AutoMapper 
    services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
    //Alternative code below: traverse the assembly and checks for the class which inherits from the Profile class of AutoMapper.
    //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 

    //to have all generated paths URLs in lower case
    services.AddRouting(options => options.LowercaseUrls = true);

    //to access the HttpContext of the request (used for serilog enricher)
    services.AddHttpContextAccessor();

    services.AddEndpointsApiExplorer();

    //configure swagger
    services.ConfigureSwagger();

    //add HealthCheck service
    services.AddHealthCheckService(configuration);
}

void ConfigureMiddleware(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.ConfigureSwagger(env);

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging(); //this adds serilog middleware to the request pipeline.
                                    //Logging will happen using serilog now and all logs by asp.net core logger will also be redirected to serilog.
                                    //That's how serilog integration doesn't affect the existing asp.net core logging 

    app.UseMiddleware<ExceptionHandlingMiddleware>(); //adding the custom middleware to the request pipeline

    app.UseRouting();

    app.UseAuthorization();    
}

void ConfigureEndpoints(IApplicationBuilder app)
{
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    //add HealthCheck endpoint
    app.ConfigureHealthCheckEndpoint();
}

