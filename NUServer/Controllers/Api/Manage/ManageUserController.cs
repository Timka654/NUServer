using Microsoft.AspNetCore.Mvc;
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
    public class ManageUserController(AppSignInManager signInManager, IConfiguration configuration, UserManager userManager, ApplicationDbContext db) : ControllerBase, IManageUserController
    {
        [HttpPostAction]
        public async Task<IActionResult> GetStorageData()
        => await this.ProcessRequestAsync(async () =>
            {
                var uid = User.GetUserId();

                var u = await signInManager.UserManager.FindByIdAsync(uid.Value.ToString());

                return this.DataResponse(new { u.Id, u.Name, u.ShareToken, u.PublishToken });
            });

        [HttpPostAction]
        public async Task<IActionResult> RefreshStoragePublishToken()
        => await this.ProcessRequestAsync(async () =>
        {
            var uid = User.GetUserId();

            var u = await signInManager.UserManager.FindByIdAsync(uid.Value.ToString());

            await userManager.SetPublishToken(db, u);

            await signInManager.UserManager.UpdateAsync(u);

            return this.DataResponse(u.PublishToken);
        });

        [HttpPostAction]
        public async Task<IActionResult> RefreshStorageShareToken()
        => await this.ProcessRequestAsync(async () =>
        {
            var uid = User.GetUserId();

            var u = await signInManager.UserManager.FindByIdAsync(uid.Value.ToString());

            await userManager.SetShareToken(db, u);

            await signInManager.UserManager.UpdateAsync(u);

            return this.DataResponse(u.ShareToken);
        });
    }
}
