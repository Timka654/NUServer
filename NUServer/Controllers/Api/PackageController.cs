using Microsoft.AspNetCore.Mvc;
using NSL.ASPNET.Mvc.Route.Attributes;
using NUServer.Api.Data;
using NUServer.Api.Managers;
using NUServer.Api.Utils.Filters;

namespace NUServer.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly PackageManager packageManager;
        private readonly ApplicationDbContext dbContext;

        public PackageController(PackageManager packageManager, ApplicationDbContext dbContext)
        {
            this.packageManager = packageManager;
            this.dbContext = dbContext;
        }

        [HttpPostAction]
        [PublishSignFilter]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Publish([FromForm(Name = "package")] IFormFile[] package, [FromHeader(Name = "uid")] string userId)
            => await packageManager.PublishPackage(ControllerContext, Url, dbContext, package, userId);

        //https://docs.microsoft.com/en-us/NuGet/api/service-index
        [HttpGet("{shareToken}/v3/index.json")]
        public async Task<IActionResult> Get(string shareToken)
            => await packageManager.GenerateNuGetIndex(dbContext, shareToken);

        //https://docs.microsoft.com/en-us/NuGet/api/search-query-service-resource
        [HttpGet("{shareToken}/query")]
        public async Task<IActionResult> Query(string shareToken, string? q, int? skip, int? take, bool? prerelease, string? semVerLevel, string? packageType)
            => await packageManager.Query(ControllerContext, dbContext, shareToken, q, skip, take, prerelease, semVerLevel, packageType);

        //https://docs.microsoft.com/en-us/NuGet/api/package-base-address-resource
        [HttpGet("{shareToken}/v3-flatcontainer/{name}/index.json")]
        public async Task<IActionResult> FlatContainer(string shareToken, string name)
            => await packageManager.GetVersionList(dbContext, shareToken, name);

        //https://docs.microsoft.com/en-us/NuGet/api/package-base-address-resource
        [HttpGet("{shareToken}/v3-flatcontainer/{name}/{version}/{name2}.{version2}.nupkg")]
        public async Task<IActionResult> FlatContainerNupkgFile(string shareToken, string name, string version)
            => await packageManager.GetNuPkgFile(dbContext, shareToken, name, version);

        //https://docs.microsoft.com/en-us/NuGet/api/package-base-address-resource
        [HttpGet("{shareToken}/v3-flatcontainer/{name}/{version}/{name2}.{version2}.nuspec")]
        public async Task<IActionResult> FlatContainerNuspecFile(string shareToken, string name, string version)
            => await packageManager.GetNuSpecFile(dbContext, shareToken, name, version);

        //https://docs.microsoft.com/en-us/NuGet/api/package-publish-resource
        [HttpPut("{shareToken}/v2/package")]
        public IActionResult Package()
        {
            return Ok();
        }

        //https://docs.microsoft.com/en-us/NuGet/api/search-autocomplete-service-resource
        [HttpGet("{shareToken}/autocomplete")]
        public async Task<IActionResult> AutoComplete(string shareToken, string? q, int? skip, int? take, bool? prerelease, string? semVerLevel, string? packageType)
            => await packageManager.AutoCompleteName(dbContext, shareToken, q, skip, take, prerelease, semVerLevel, packageType);

        //https://docs.microsoft.com/en-us/NuGet/api/registration-base-url-resource
        [HttpGet("{shareToken}/v3/registration3/{name}/index.json")]
        public async Task<IActionResult> Registration(string shareToken, string name)
            => await packageManager.Registration(ControllerContext, Url, dbContext, shareToken, name);

    }
}
