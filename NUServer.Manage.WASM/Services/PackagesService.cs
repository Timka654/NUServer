using NSL.Generators.HttpEndPointGenerator.Shared.Attributes;
using NUServer.Shared.Models.Controllers;

namespace NUServer.Manage.WASM.Services
{
    [HttpEndPointImplementGenerate(typeof(IManagePackagesController))]
    public partial class PackagesService(IHttpClientFactory httpClientFactory)
    {
        protected partial HttpClient CreateEndPointClient(string url)
            => httpClientFactory.CreateClient("Default");
    }
}
