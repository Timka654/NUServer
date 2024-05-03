using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NU.Core.Models.Response
{
    public class NuGetRegistrationCatalogDepedencyGroupModel
    {
        //"https://api.NuGet.org/v3/catalog0/data/2017.10.05.18.41.33/NuGet.server.core.3.0.0-beta.json#dependencygroup"
        [JsonPropertyName("@id")]
        public virtual string Url { get; set; }

        public virtual string TargetFramework { get; set; }

        public virtual List<NuGetRegistrationCatalogDepedencyModel> Dependencies { get; set; }
    }
}
