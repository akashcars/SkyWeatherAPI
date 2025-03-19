using DotNetEnv;
using Microsoft.OpenApi.Models;
using SkyWeatherAPI;
using SkyWeatherAPI.Services;
using System.Reflection;
using System.Text.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureCors(); //--- Set CORS policy

        // Add services to the container.
        builder.Services.AddDistributedMemoryCache(); // Or use a different session store
        builder.Services.AddMemoryCache();

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
            options.Cookie.HttpOnly = true; // Ensures the session cookie is accessible only by the server
            options.Cookie.IsEssential = true; // Required for GDPR compliance
        });

        // Load environment variables from the .env file
        Env.Load();

        // Add services to the container.

        //to set camel case for web api object property names
        builder.Services.AddControllers()
             .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase); //--- Set JSON naming policy

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Sky Weather API",
                Description = "Weather API Description", 
            });

            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        builder.Services.AddScoped<IWeatherService, WeatherService>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseSession();
        app.UseCors("SkyWeatherAppPolicy");

        app.MapControllers();

        app.Run();
    }
}