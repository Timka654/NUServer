using NU.Core.Models.Response;
using NUServer.Shared.Models;
using System.Text.Json.Serialization;

namespace NUServer.Models.Response
{
    public class NuGetRegistrationCatalogEntryServerModel : NuGetRegistrationCatalogEntryModel
    {
        [JsonPropertyName("@id")]
        public override string Url { get; set; }

        [JsonPropertyName("packageContent")]
        public override string PackageContentUrl { get; set; }

        public NuGetRegistrationCatalogEntryServerModel(PackageModel package, PackageVersionModel version, Func<string, string?, string> registrationUrl, Func<string, string?, string> nupkgUrl)
        {
            Authors = package.Author.UserName;
            Description = package.Description;
            Id = package.Name;

            PackageContentUrl = nupkgUrl(package.Name, version.Version);
            Version = version.Version;
            Published = version.UploadTime;

            DependencyGroups = new List<NuGetRegistrationCatalogDepedencyGroupModel>(version.DepedencyGroupList.Select(x => new NuGetRegistrationCatalogDependencyGroupServerModel(package, version, x, registrationUrl)));
        }
    }
}
