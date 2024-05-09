using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSL.ASPNET.Identity.Host;
using NSL.ASPNET.Mvc;
using NSL.ASPNET.Mvc.Route.Attributes;
using NSL.Database.EntityFramework.Filter;
using NSL.Database.EntityFramework.Filter.Host;
using NSL.Database.EntityFramework.Filter.Models;
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
    public class ManagePackagesController(ApplicationDbContext dbContext
        , PackageManager packageManager) : ControllerBase, IManagePackagesController
    {
        [HttpPostAction]
        public async Task<IActionResult> Get([FromBody] BaseFilteredQueryModel query)
        => await this.ProcessRequestAsync(async () =>
        {
            var uid = User.GetUserId();

            return this.DataResponse(await dbContext.Packages
                .Filter(q => q.Where(x => x.AuthorId == uid), query)
                .ToDataResultAsync(s => s.SelectGet()));
        });

        [HttpPostAction]
        public async Task<IActionResult> Details([FromBody] Guid query)
        => await this.ProcessRequestAsync(async () =>
        {
            var uid = User.GetUserId();

            var data = await dbContext.Packages
            .Include(x => x.VersionList)
            .Where(x => x.AuthorId == uid && x.Id == query)
            .SelectGetDetails()
            .FirstOrDefaultAsync();

            if (data == null)
                return this.NotFoundResponse();

            return this.DataResponse((object)data);
        });

        [HttpPostAction]
        public async Task<IActionResult> UploadPackage([FromForm] IFormFile query)
        => await this.ProcessRequestAsync(async () =>
        {
            var uid = User.GetUserId();

            var user = await dbContext.Users.FindAsync(uid.Value);

            var result = await packageManager.PublishPackage(dbContext, query, user);

            if (result.Package == null)
                return this.ModelStateResponse(result.ErrorField, result.ErrorMessage);

            return this.DataResponse((object)await dbContext.Packages
                .Where(x => x.AuthorId == uid && x.Id == result.Package.Id)
                .SelectGet()
                .FirstOrDefaultAsync());
        });

        [HttpPostAction]
        public async Task<IActionResult> RemoveVersion([FromForm] RemovePackageVersionRequestModel query)
        => await this.ProcessRequestAsync(async () =>
        {
            var uid = User.GetUserId();

            await dbContext.PackageVersions
            .Include(x => x.Package)
            .Where(x =>
            x.Package.AuthorId == uid.Value
            && x.PackageId == query.PackageId
            && x.Version == query.PackageVersion)
            .ExecuteDeleteAsync();


            return Ok();
        });

        [HttpPostAction]
        public async Task<IActionResult> Remove([FromForm] Guid query)
        => await this.ProcessRequestAsync(async () =>
        {
            var uid = User.GetUserId();

            await dbContext.Packages
            .Where(x =>
            x.AuthorId == uid.Value
            && x.Id == query)
            .ExecuteDeleteAsync();

            return Ok();
        });
    }
}
