using System.Text.Json.Serialization;
using CarShopApi.Services;
using Microsoft.EntityFrameworkCore;

namespace CarShopApi.Config;

public class DependencyInjector
{
    public static void Inject(IServiceCollection services, IConfiguration configuration)
    {
        // Build connection string from environment variables if in production
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            // Build connection string from individual environment variables for production
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
            var database = Environment.GetEnvironmentVariable("DB_NAME");
            var username = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            
            if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(database) && 
                !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";
            }
        }

        services.AddDbContext<AppDbContext>(options =>  
            options.UseNpgsql(connectionString)
        );

        services.AddControllers().AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.AddSwaggerGen();
        services.AddRouting(opt =>
        {
            opt.LowercaseUrls = true;
        });
        services.AddScoped<UserService>();
        services.AddScoped<CarService>();   
        services.AddScoped<OrderService>();
        services.AddScoped<ComissionService>();
    }
}