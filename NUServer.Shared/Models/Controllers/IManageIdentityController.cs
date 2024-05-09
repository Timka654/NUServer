#if SERVER
using Microsoft.AspNetCore.Mvc;
#endif

using NSL.Generators.HttpEndPointGenerator.Shared.Attributes;
using NSL.HttpClient.Models;
using NUServer.Shared.Models.Request;
using NUServer.Shared.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUServer.Shared.Models.Controllers
{
    [HttpEndPointContainerGenerate("api/manage/[controller]")]
    public interface IManageIdentityController
    {

        [HttpEndPointGenerate(typeof(DataResponse<SignResponseModel>))] Task<IActionResult> SignIn([FromBody] SignInRequestModel query);

        [HttpEndPointGenerate(typeof(DataResponse<SignResponseModel>))] Task<IActionResult> SignUp([FromBody] SignUpRequestModel query);
    }
}
