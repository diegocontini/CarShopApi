using System.Text.Json.Serialization;
using CarShopApi;
using CarShopApi.Config;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
DependencyInjector.Inject(builder.Services, builder.Configuration);



var app = builder.Build();



app.MapControllers();
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});



app.Run();
