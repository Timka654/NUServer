using NU.Core.Models.Response;
using NUServer.Models;
using System.Text.Json.Serialization;

namespace NUServer.Api.Models.Response
{
    public class NugetRegistrationLeafServerModel : NugetRegistrationLeafModel
    {
        [JsonPropertyName("@id")]
        public override string Url { get; set; }

        public NugetRegistrationLeafServerModel(PackageModel package, PackageVersionModel version, Func<string, string?, string> registrationUrl, Func<string, string?, string> nupkgUrl)
        {
            Url = registrationUrl(package.Name, version.Version);
            Registration = registrationUrl(package.Name, null);

            PackageContent = nupkgUrl(package.Name, version.Version);

            CatalogEntry = new NugetRegistrationCatalogEntryServerModel(package, version, registrationUrl, nupkgUrl);

        }
    }
}
