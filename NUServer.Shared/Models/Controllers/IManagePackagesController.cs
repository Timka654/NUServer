#if SERVER
using Microsoft.AspNetCore.Mvc;
#endif

using NSL.Database.EntityFramework.Filter.Models;
using NSL.Generators.HttpEndPointGenerator.Shared.Attributes;
using NSL.HttpClient.Models;
using NUServer.Shared.Models.Request;
using NUServer.Shared.Models.Response;

namespace NUServer.Shared.Models.Controllers
{
    [HttpEndPointContainerGenerate("api/manage/[controller]")]
    public interface IManagePackagesController
    {
        [HttpEndPointGenerate(typeof(DataResponse<PackageModel>))] Task<IActionResult> Details([FromBody] Guid query);

        [HttpEndPointGenerate(typeof(DataResponse<FilterResultModel<PackageModel>>))] Task<IActionResult> Get([FromBody] BaseFilteredQueryModel query);

        [HttpEndPointGenerate(typeof(BaseResponse))] Task<IActionResult> Remove([FromBody] Guid query);

        [HttpEndPointGenerate(typeof(DataResponse<bool>))] Task<IActionResult> RemoveVersion([FromBody] RemovePackageVersionRequestModel query);

        [HttpEndPointGenerate(typeof(DataResponse<PackageModel>))] Task<IActionResult> UploadPackage([FromForm] IFormFile query);
    }
}
