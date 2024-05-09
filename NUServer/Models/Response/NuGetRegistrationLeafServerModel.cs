using NU.Core.Models.Response;
using NUServer.Shared.Models;
using System.Text.Json.Serialization;

namespace NUServer.Models.Response
{
    public class NuGetRegistrationLeafServerModel : NuGetRegistrationLeafModel
    {
        [JsonPropertyName("@id")]
        public override string Url { get; set; }

        public NuGetRegistrationLeafServerModel(PackageModel package, PackageVersionModel version, Func<string, string?, string> registrationUrl, Func<string, string?, string> nupkgUrl)
        {
            Url = registrationUrl(package.Name, version.Version);
            Registration = registrationUrl(package.Name, null);

            PackageContent = nupkgUrl(package.Name, version.Version);

            CatalogEntry = new NuGetRegistrationCatalogEntryServerModel(package, version, registrationUrl, nupkgUrl);

        }
    }
}
