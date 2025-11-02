using System.Text.Json.Serialization;
using CarShopApi.Services;
using Microsoft.EntityFrameworkCore;

namespace CarShopApi.Config;

public class DependencyInjector
{
    public static void Inject(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>  
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
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
    }
}