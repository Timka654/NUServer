using NU.Core.Models.Response;
using NUServer.Shared.DB;
using System.Text.Json.Serialization;

namespace NUServer.Api.Models.Response
{
    public class NuGetRegistrationCatalogDependencyGroupServerModel : NuGetRegistrationCatalogDepedencyGroupModel
    {
        [JsonPropertyName("@id")]
        public override string Url { get; set; }

        public NuGetRegistrationCatalogDependencyGroupServerModel(PackageModel package, PackageVersionModel version, PackageVersionDepedencyGroupModel group, Func<string, string?, string> registrationUrl)
        {
            TargetFramework = group.TargetFramework;
            Dependencies = new List<NuGetRegistrationCatalogDepedencyModel>(group.Depedencies.Select(x => new NuGetRegistrationCatalogDepedencyServerModel(x, package, version, registrationUrl)));
        }
    }
}
