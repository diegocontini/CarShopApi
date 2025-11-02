using System.Text.Json.Serialization;
using CarShopApi;
using CarShopApi.Config;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on the correct port
builder.WebHost.ConfigureKestrel(options =>
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    options.ListenAnyIP(int.Parse(port));
});

DependencyInjector.Inject(builder.Services, builder.Configuration);

var app = builder.Build();

app.MapControllers();

// Add health check endpoint for Koyeb
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
        // Don't exit the application, let it continue running
        // The health check will still work even if DB initialization fails
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Remove HTTPS redirection for containerized deployment
// app.UseHttpsRedirection();

app.MapGet("/", context =>
{
    if (app.Environment.IsDevelopment())
    {
        context.Response.Redirect("/swagger");
    }
    else
    {
        context.Response.Redirect("/health");
    }
    return Task.CompletedTask;
});

app.Run();
