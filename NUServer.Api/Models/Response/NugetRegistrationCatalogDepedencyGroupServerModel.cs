using NUServer.Models;
using NU.Core.Models.Response;
using System.Text.Json.Serialization;
using NUServer.Models.DB;

namespace NUServer.Api.Models.Response
{
    public class NugetRegistrationCatalogDepedencyGroupServerModel : NugetRegistrationCatalogDepedencyGroupModel
    {
        [JsonPropertyName("@id")]
        public override string Url { get; set; }

        public NugetRegistrationCatalogDepedencyGroupServerModel(PackageModel package, PackageVersionModel version, PackageVersionDepedencyGroupModel group, Func<string, string?, string> registrationUrl)
        {
            TargetFramework = group.TargetFramework;
            Dependencies = group.Depedencies.Select(x => new NugetRegistrationCatalogDepedencyServerModel(x, package, version, registrationUrl)).ToArray();
        }
    }
}
