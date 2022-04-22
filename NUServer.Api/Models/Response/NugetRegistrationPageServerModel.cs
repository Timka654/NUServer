using NU.Core.Models.Response;
using System.Text.Json.Serialization;
using NUServer.Models.DB;

namespace NUServer.Api.Models.Response
{
    public class NugetRegistrationPageServerModel : NugetRegistrationPageModel
    {
        [JsonPropertyName("@id")]
        public override string Url { get; set; }

        public NugetRegistrationPageServerModel(PackageModel package, Func<string, string?, string> registrationUrl, Func<string, string?, string> nupkgUrl)
        {
            Items = new List<NugetRegistrationLeafModel>(package.VersionList.Select(x => new NugetRegistrationLeafServerModel(package, x, registrationUrl, nupkgUrl)));

            Lower = package.VersionList.OrderBy(x => x.UploadTime).First().Version;

            Upper = package.LatestVersion;

            Url = registrationUrl(package.Name, null) + $"#page/{Upper}/{Upper}";
        }
    }
}
