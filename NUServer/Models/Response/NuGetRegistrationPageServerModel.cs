using NU.Core.Models.Response;
using NUServer.Shared.DB;
using System.Text.Json.Serialization;

namespace NUServer.Api.Models.Response
{
    public class NuGetRegistrationPageServerModel : NuGetRegistrationPageModel
    {
        [JsonPropertyName("@id")]
        public override string Url { get; set; }

        public NuGetRegistrationPageServerModel(PackageModel package, Func<string, string?, string> registrationUrl, Func<string, string?, string> nupkgUrl)
        {
            Items = new List<NuGetRegistrationLeafModel>(package.VersionList.Select(x => new NuGetRegistrationLeafServerModel(package, x, registrationUrl, nupkgUrl)));

            Lower = package.VersionList.OrderBy(x => x.UploadTime).First().Version;

            Upper = package.LatestVersion;

            Url = registrationUrl(package.Name, null) + $"#page/{Upper}/{Upper}";
        }
    }
}
