using NU.Core.Models.Response;
using NUServer.Shared.DB;

namespace NUServer.Api.Models.Response
{
    public class NuGetRegistrationResponseServerModel : NuGetRegistrationResponseModel
    {
        public NuGetRegistrationResponseServerModel(PackageModel package, Func<string, string?, string> registrationUrl, Func<string, string?, string> nupkgUrl)
        {
            Items = new List<NuGetRegistrationPageModel> { new NuGetRegistrationPageServerModel(package, registrationUrl, nupkgUrl) };
        }
    }
}
