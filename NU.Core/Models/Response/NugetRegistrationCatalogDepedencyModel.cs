using System.Text.Json.Serialization;

namespace NU.Core.Models.Response
{
    public class NuGetRegistrationCatalogDepedencyModel
    {
        //"https://api.NuGet.org/v3/catalog0/data/2017.10.05.18.41.33/NuGet.server.core.3.0.0-beta.json#dependencygroup/NuGet.core"
        [JsonPropertyName("@id")]
        public virtual string Url { get; set; }

        //"NuGet.Core",
        [JsonPropertyName("id")]
        public virtual string Name { get; set; }

        //"[2.14.0, )"
        public virtual string Range { get; set; }

        //"https://api.NuGet.org/v3/registration3/NuGet.core/index.json"
        public virtual string Registration { get; set; }
    }
}
