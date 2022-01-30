using Microsoft.OpenApi.Models;
using VaccineInfo.Api.Mappings;
using VaccineInfo.API;

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

    //SuppressAsyncSuffixInActionNames: to be able to find the action method without "Async" suffix at runtime, at the case when that action is called from another action method 
    //AddNewtonsoftJson: this is for PATCH using JsonPatchDocument
    services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false).AddNewtonsoftJson();

    //Register AutoMapper
    services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
    //Alternative code below: traverse the assembly and checks for the class which inherits from the Profile class of AutoMapper.
    //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "VaccineInfo.Api", Version = "V1" });
    }).AddSwaggerGenNewtonsoftSupport();
}

void ConfigureMiddleware(IApplicationBuilder app, IWebHostEnvironment env) 
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VaccineInfo.Api V1"));
    }
    else
    {
        app.UseExceptionHandler("/error");
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();
}

void ConfigureEndpoints(IEndpointRouteBuilder app)
{
    app.MapControllers();
}

