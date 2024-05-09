using Microsoft.AspNetCore.Identity;
using NSL.ASPNET.Identity.Host;
using NUServer.Data;
using NUServer.Shared.Models;
using NUServer.Utils.Identity;
using System.Net;

namespace NUServer
{
    internal partial class Program
	{
		private const string AdminRoleName = "PlatformAdministrator";
		private const string AdminUserName = "admin@platform.admin";
		private const string AdminPassword = "000000";

		private static IServiceCollection AddIdentity(IServiceCollection services, IConfiguration configuration)
		{
			services.ConfigureAPIBaseJWTData(configuration)
				.AddAPIBaseIdentity<UserModel, IdentityRole<Guid>>()
				.AddDefaultTokenProviders()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddUserManager<AppUserManager>()
				.AddRoleManager<AppRoleManager>()
				.AddSignInManager<AppSignInManager>()
				.Services

				.AddDefaultAuthenticationForAPIBaseJWT()
				.AddAPIBaseJWTBearer(configuration)
				.Services

				.ConfigureApplicationCookie(c =>
				{
					c.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents()
					{
						OnRedirectToAccessDenied = c =>
						{
							c.Response.StatusCode = (int)HttpStatusCode.Forbidden;

							return Task.CompletedTask;
						},
						OnRedirectToLogin = c =>
						{
							c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

							return Task.CompletedTask;
						}
					};
				})
				.AddAuthorization(c =>
				{
					c.AddPolicy("PlatformAdministrator", c =>
					{
						c.RequireRole(AdminRoleName);
					});
				});

			return services;
		}

		private static async Task<IServiceProvider> LoadIdentity(IServiceProvider sp)
		{
			await using var _serviceProvider = sp.CreateAsyncScope();

			var serviceProvider = _serviceProvider.ServiceProvider;

			var roleManager = serviceProvider.GetRequiredService<AppRoleManager>();

			if (!await roleManager.RoleExistsAsync(AdminRoleName))
			{
				var result = await roleManager.CreateAsync(new IdentityRole<Guid>() { Name = AdminRoleName });

				if (!result.Succeeded)
					throw new InvalidOperationException($"Cannot initialize identity - {string.Join("\r\n", result.Errors.Select(x => x.Description))}");
			}

			var userManager = serviceProvider.GetRequiredService<AppUserManager>();

			var user = await userManager.FindByEmailAsync(AdminUserName);

			if (user == null)
			{
				user = new UserModel() { Email = AdminUserName, UserName = AdminUserName, Name = string.Empty };

				var result = await userManager.CreateAsync(user, AdminPassword);

				if (!result.Succeeded)
					throw new InvalidOperationException($"Cannot initialize identity - {string.Join("\r\n", result.Errors.Select(x => x.Description))}");
			}

			if (!await userManager.IsInRoleAsync(user, AdminRoleName))
			{
				var result = await userManager.AddToRoleAsync(user, AdminRoleName);

				if (!result.Succeeded)
					throw new InvalidOperationException($"Cannot initialize identity - {string.Join("\r\n", result.Errors.Select(x => x.Description))}");
			}

			return sp;
		}
	}
}
