using Microsoft.Extensions.Options;

namespace SkyWeatherAPI
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "SkyWeatherAppPolicy",
                                            policy =>
                                            {
                                                policy.WithOrigins( "http://localhost:5173")
                                                                      .AllowAnyHeader()
                                                                      .AllowAnyMethod();
                                            })
                          ;
            });
        }
    }
}

 