using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NU.Core.Models.Response
{
    public class NugetRegistrationCatalogDepedencyModel
    {
        //"https://api.nuget.org/v3/catalog0/data/2017.10.05.18.41.33/nuget.server.core.3.0.0-beta.json#dependencygroup/nuget.core"
        [JsonPropertyName("@id")]
        public virtual string Url { get; set; }

        //"NuGet.Core",
        [JsonPropertyName("id")]
        public virtual string Name { get; set; }

        //"[2.14.0, )"
        public virtual string Range { get; set; }

        //"https://api.nuget.org/v3/registration3/nuget.core/index.json"
        public virtual string Registration { get; set; }
    }
}
