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
    public interface IManagePackagesController
    {

        [HttpEndPointGenerate(typeof(DataResponse<SignResponseModel>))] Task<IActionResult> SignIn([FromBody] SignInRequestModel query);

        [HttpEndPointGenerate(typeof(DataResponse<SignResponseModel>))] Task<IActionResult> SignUp([FromBody] SignUpRequestModel query);
    }
}
