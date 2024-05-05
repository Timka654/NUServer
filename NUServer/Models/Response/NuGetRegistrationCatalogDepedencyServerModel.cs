using NU.Core.Models.Response;
using System.Text.Json.Serialization;
using NuGet.Versioning;
using NUServer.Shared.DB;

namespace NUServer.Api.Models.Response
{
    public class NuGetRegistrationCatalogDepedencyServerModel : NuGetRegistrationCatalogDepedencyModel
    {
        [JsonPropertyName("@id")]
        public override string Url { get; set; }

        [JsonPropertyName("id")]
        public override string Name { get; set; }

        public NuGetRegistrationCatalogDepedencyServerModel(PackageVersionDependencyModel x, PackageModel package, PackageVersionModel version, Func<string, string?, string> registrationUrl)
        {
            Registration = registrationUrl(x.DependencyName, version.Version);
            Name = x.DependencyName;
            Range = VersionRange.Parse(x.DependencyVersion).ToString();
        }
    }
}
