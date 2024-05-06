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
    }
}
