using System.Text.Json.Serialization;
using CarShopApi;
using CarShopApi.Config;
using CarShopApi.Models;
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
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    try
    {
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
      

        
        var hasAdmin = context.Users.Any(u => u.Username == "admin");
        if (!hasAdmin)
        {
            var adminUsername = configuration["SuperUser:Username"] ?? "admin";
            var adminPassword = configuration["SuperUser:Password"] ?? "admin";
            var adminEmail = configuration["SuperUser:Email"] ?? "admin@localhost";

            var superUser = new User
            {
                Username = adminUsername,
                Password = adminPassword,
                Email = adminEmail,
                ComissionPerSaleInPercent = 3,
                Role = UserRole.Admin
            };

            context.Users.Add(superUser);
            context.SaveChanges();
            Console.WriteLine($"Super user created: {adminUsername}");
        }
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
