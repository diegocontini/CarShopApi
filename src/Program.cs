using System.Text.Json.Serialization;
using CarShopApi;
using CarShopApi.Config;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureKestrel(options =>
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    options.ListenAnyIP(int.Parse(port));
});

DependencyInjector.Inject(builder.Services, builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        context.Database.EnsureCreated();
        context.Database.Migrate();
        
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error initializing database: {ex.Message}");
    }
}

    app.UseSwagger();
    app.UseSwaggerUI();


app.MapGet("/", context =>
{
    
        context.Response.Redirect("/swagger");
        
    return Task.CompletedTask;
});

app.Run();
