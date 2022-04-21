using NUServer.Models;
using NU.Core.Models.Response;

namespace NUServer.Api.Models.Response
{
    public class NugetRegistrationResponseServerModel : NugetRegistrationResponseModel
    {
        public NugetRegistrationResponseServerModel(PackageModel package, Func<string, string?, string> registrationUrl, Func<string, string?, string> nupkgUrl)
        {
            Items = new NugetRegistrationPageServerModel[] { new NugetRegistrationPageServerModel(package, registrationUrl, nupkgUrl) };
        }
    }
}
