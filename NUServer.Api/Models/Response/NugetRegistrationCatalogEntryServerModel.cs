using NUServer.Models;
using NU.Core.Models.Response;
using System.Text.Json.Serialization;

namespace NUServer.Api.Models.Response
{
    public class NugetRegistrationCatalogEntryServerModel : NugetRegistrationCatalogEntryModel
    {
        [JsonPropertyName("@id")]
        public override string Url { get; set; }

        [JsonPropertyName("packageContent")]
        public override string PackageContentUrl { get; set; }

        public NugetRegistrationCatalogEntryServerModel(PackageModel package, PackageVersionModel version, Func<string, string?, string> registrationUrl, Func<string, string?, string> nupkgUrl)
        {
            Authors = package.Avtor.Name;
            Description = package.Description;
            Id = package.Name;

            PackageContentUrl = nupkgUrl(package.Name, version.Version);
            Version = version.Version;
            Published = version.UploadTime;

            DependencyGroups = version.DepedencyGroupList.Select(x => new NugetRegistrationCatalogDepedencyGroupServerModel(package, version, x, registrationUrl)).ToArray();
        }
    }
}
