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
                c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddRazorPages();


            var app = builder.Build();

            app.UseHttpLogging();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                db.Database.Migrate();
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