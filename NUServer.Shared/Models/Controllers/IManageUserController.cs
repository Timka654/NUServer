#if SERVER
using Microsoft.AspNetCore.Mvc;
#endif

using NSL.Generators.HttpEndPointGenerator.Shared.Attributes;
using NSL.HttpClient.Models;
using NUServer.Shared.Models.Request;
using NUServer.Shared.Models.Response;

namespace NUServer.Shared.Models.Controllers
{
    [HttpEndPointContainerGenerate("api/manage/[controller]")]
    public interface IManageUserController
    {
        [HttpEndPointGenerate(typeof(DataResponse<UserModel>))] Task<IActionResult> GetStorageData();

        [HttpEndPointGenerate(typeof(DataResponse<string>))] Task<IActionResult> RefreshStorageShareToken();

        [HttpEndPointGenerate(typeof(DataResponse<string>))] Task<IActionResult> RefreshStoragePublishToken();
    }
}
