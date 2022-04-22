using NU.Core.Models.Response;
using System.Text.Json.Serialization;
using NUServer.Models.DB;
using NuGet.Versioning;

namespace NUServer.Api.Models.Response
{
    public class NugetRegistrationCatalogDepedencyServerModel : NugetRegistrationCatalogDepedencyModel
    {
        [JsonPropertyName("@id")]
        public override string Url { get; set; }

        [JsonPropertyName("id")]
        public override string Name { get; set; }

        public NugetRegistrationCatalogDepedencyServerModel(PackageVersionDepedencyModel x, PackageModel package, PackageVersionModel version, Func<string, string?, string> registrationUrl)
        {
            Registration = registrationUrl(x.DepedencyName, version.Version);
            Name = x.DepedencyName;
            Range = VersionRange.Parse(x.DepedencyVersion).ToString();
        }
    }
}
