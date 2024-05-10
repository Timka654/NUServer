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
        public async Task<IActionResult> RemoveVersion([FromBody] RemovePackageVersionRequestModel query)
        => await this.ProcessRequestAsync(async () =>
        {
            var uid = User.GetUserId();

            var version =

            await dbContext.PackageVersions
            .Include(x => x.Package)
            .FirstOrDefaultAsync(x =>
            x.Package.AuthorId == uid.Value
            && x.PackageId == query.PackageId
            && x.Version == query.PackageVersion);

            bool fullRemove = false;

            if (version != null)
            {
                var package = version.Package;

                await packageManager.RemovePackageVerFiles(version);

                dbContext.Entry(version).State = EntityState.Deleted;

                if (package.VersionList.Count > 1)
                {
                    var ver = package.VersionList.Where(x => x.Version != version.Version).OrderByDescending(x => x.UploadTime).First();

                    package.LatestVersion = ver.Version;
                }
                else
                {
                    dbContext.Entry(package).State = EntityState.Deleted;
                    fullRemove = true;
                }

                await dbContext.SaveChangesAsync();
            }

            return this.DataResponse(fullRemove);
        });

        [HttpPostAction]
        public async Task<IActionResult> Remove([FromBody] Guid query)
        => await this.ProcessRequestAsync(async () =>
        {
            var uid = User.GetUserId();

            var package =

             await dbContext.Packages
            .Include(x => x.VersionList)
            .FirstOrDefaultAsync(x =>
            x.AuthorId == uid.Value
            && x.Id == query);

            if (package != null)
            {
                await packageManager.RemovePackageFiles(package);

                dbContext.Entry(package).State = EntityState.Deleted;

                await dbContext.SaveChangesAsync();
            }

            return Ok();
        });
    }
}
