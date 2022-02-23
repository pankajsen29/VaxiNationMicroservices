using WeatherForecastRemote.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
ConfigureMiddleware(app, app.Environment);
ConfigureEndpoints(app);

app.Run();



void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    //dependencies are injected in separate extension method
    services.ConfigureDependencyInjection(configuration);

    builder.Services.AddControllers();

    //to have all generated paths URLs in lower case
    services.AddRouting(options => options.LowercaseUrls = true);

    services.AddEndpointsApiExplorer();

    //configure swagger
    services.AddSwaggerGen();
}

void ConfigureMiddleware(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Configure the HTTP request pipeline.
    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();
}

void ConfigureEndpoints(IApplicationBuilder app)
{
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
