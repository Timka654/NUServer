using Microsoft.EntityFrameworkCore;
using NUServer.Api.Data;
using NUServer.Api.Managers;
using NUServer.Core;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(c => c.UseNpgsql(builder.Configuration.GetConnectionString("db")));

builder.Services.AddSingleton<PackageManager>();

builder.Services.AddSingleton<UserManager>();


builder.Services.AddControllers().
    AddJsonOptions(c => { 
        c.JsonSerializerOptions.WriteIndented = true; 
        c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; 
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    db.Database.Migrate();
}

// Configure the HTTP request pipeline.

app.UseStaticFiles();

app.UseHttpLogging();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
