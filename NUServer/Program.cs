using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using NSL.ASPNET.Identity.Host;
using NUServer.Data;
using NUServer.Managers;
using System.Text.Json.Serialization;

namespace NUServer
{
    internal partial class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddHttpLogging(c =>
            {
            });

            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                      new[] { "application/octet-stream" });
            });
            // Add services to the container.

            builder.Services.AddDbContext<ApplicationDbContext>(c => c.UseNpgsql(builder.Configuration.GetConnectionString("db")));

            builder.Services.AddSingleton<PackageManager>();

            builder.Services.AddSingleton<UserManager>();

            AddIdentity(builder.Services, builder.Configuration);

            builder.Services.AddControllers().
            AddJsonOptions(c =>
            {
                c.JsonSerializerOptions.WriteIndented = true;
                c.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });

            builder.Services.AddRazorPages();


            var app = builder.Build();

            app.UseHttpLogging();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await db.Database.MigrateAsync();

                if (!await db.Resources.AnyAsync())
                {
                    //min default list for correct working
                    db.Resources.Add(new Shared.Models.ResourceModel() { Id = 1, Active = true, Url = "/api/Package/{shareToken}/v3/registration3/", Type = "RegistrationsBaseUrl/3.0.0-beta", Comment = "Base URL of Azure storage where NuGet package registration info is stored used by RC clients. This base URL does not include SemVer 2.0.0 packages." });
                    db.Resources.Add(new Shared.Models.ResourceModel() { Id = 2, Active = true, Url = "/api/Package/{shareToken}/v2/package", Type = "PackagePublish/2.0.0", Comment = "" });
                    db.Resources.Add(new Shared.Models.ResourceModel() { Id = 3, Active = true, Url = "/api/Package/{shareToken}/v3-flatcontainer", Type = "PackageBaseAddress/3.0.0", Comment = "Base URL of where NuGet packages are stored, in the format https://api.nuget.org/v3-flatcontainer/{id-lower}/{version-lower}/{id-lower}.{version-lower}.nupkg" });
                    db.Resources.Add(new Shared.Models.ResourceModel() { Id = 4, Active = true, Url = "/api/Package/{shareToken}/autocomplete", Type = "SearchAutocompleteService/3.0.0-rc", Comment = "Autocomplete endpoint of NuGet Search service (primary) used by RC clients" });
                    db.Resources.Add(new Shared.Models.ResourceModel() { Id = 5, Active = true, Url = "/api/Package/{shareToken}/query", Type = "SearchQueryService/Versioned", Comment = "Query endpoint of NuGet Search service (primary)" });

                    await db.SaveChangesAsync();
                }
            }

            await LoadIdentity(app.Services);


            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Configure the HTTP request pipeline.

            app.UseStaticFiles();

            app.UseHttpLogging();

            //app.UseHttpsRedirection();

            app.UseAuth();

            app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/manage"), app1 =>
            {
                if (app.Environment.IsDevelopment())
                {
                    app1.UseWebAssemblyDebugging();
                }

                app1.UseBlazorFrameworkFiles("/manage");
                app1.UseRouting();
                app1.UseEndpoints(endpoints =>
                {
                    endpoints.MapFallbackToPage("/manage/{*path:nonfile}", "/Manage");
                });
            });

            app.MapControllers();

            app.MapRazorPages();

            app.Run();
        }
    }
}