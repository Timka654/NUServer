using NUServer.Models;
using NU.Core.Models.Response;
using System.Text.Json.Serialization;

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
            Registration = registrationUrl(package.Name, version.Version);
            Name = x.DepedencyName;
            Range = $"[{x.DepedencyVersion}, )";
        }
    }
}
