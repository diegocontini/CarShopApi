using CarShopApi;
using Microsoft.EntityFrameworkCore;

public class CarEndpoints
{
    public static void Map(WebApplication app)
    {
        // Add a simple endpoint to test database connectivity
        app.MapGet(
                "/cars",
                async (AppDbContext context) =>
                {
                    var result = await context.Cars.ToListAsync();

                    return result;
                }
            )
            .WithName("GetCars")
            .WithOpenApi();

        app.MapPost(
                "/cars",
                async (Car car, AppDbContext context) =>
                {
                    context.Cars.Add(car);
                    await context.SaveChangesAsync();
                    return Results.Created($"/cars/{car.Id}", car);
                }
            )
            .WithName("CreateCar")
            .WithOpenApi();
    }
}
