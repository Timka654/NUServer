﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSL.ASPNET.Identity.Host;
using NSL.ASPNET.Mvc;
using NSL.ASPNET.Mvc.Route.Attributes;
using NUServer.Data;
using NUServer.Managers;
using NUServer.Shared.Models;
using NUServer.Shared.Models.Controllers;
using NUServer.Shared.Models.Request;
using NUServer.Utils.Identity;

namespace NUServer.Controllers.Api.Manage
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class ManageIdentityController(AppSignInManager signInManager, IConfiguration configuration, UserManager userManager, ApplicationDbContext db) : ControllerBase, IManageIdentityController
    {
        [HttpPostAction]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestModel query)
        => await this.ProcessRequestAsync(async () =>
            {
                var u = await signInManager.UserManager.FindByEmailAsync(query.Email);

                if (u == null)
                    return this.ModelStateResponse("User not found");

                if (!await signInManager.UserManager.CheckPasswordAsync(u, query.Password))
                    return this.ModelStateResponse("User not found");

                var token = u.GenerateClaims((_u, claims) =>
                {
                }).GenerateJWT(configuration);

                return this.DataResponse(new { token });
            });

        [HttpPostAction]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestModel query)
        => await this.ProcessRequestAsync(async () =>
        {
            if (await signInManager.UserManager.Users.AnyAsync(x => x.Name.ToLower() == query.Name.ToLower()))
                return this.ModelStateResponse($"User {query.Name} already exists");

            var u = new UserModel() { Email = query.Email, UserName = query.Email };

            query.FillTo(u);

            await userManager.SetTokens(db, u);

            var r = await signInManager.UserManager.CreateAsync(u, query.Password);

            if (!r.Succeeded)
            {
                return this.ModelStateResponse(r.Errors.Select(x => x.Description).ToArray());
            }

            var token = u.GenerateClaims((_u, claims) =>
            {
            }).GenerateJWT(configuration);

            return this.DataResponse(new { token });
        });
    }
}
