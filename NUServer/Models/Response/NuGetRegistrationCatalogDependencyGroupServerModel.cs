using NU.Core.Models.Response;
using NUServer.Shared.Models;
using System.Text.Json.Serialization;

namespace NUServer.Models.Response
{
    public class NuGetRegistrationCatalogDependencyGroupServerModel : NuGetRegistrationCatalogDepedencyGroupModel
    {
        [JsonPropertyName("@id")]
        public override string Url { get; set; }

        public NuGetRegistrationCatalogDependencyGroupServerModel(PackageModel package, PackageVersionModel version, PackageVersionDependencyGroupModel group, Func<string, string?, string> registrationUrl)
        {
            TargetFramework = group.TargetFramework;
            Dependencies = new List<NuGetRegistrationCatalogDepedencyModel>(group.Dependencies.Select(x => new NuGetRegistrationCatalogDepedencyServerModel(x, package, version, registrationUrl)));
        }
    }
}
