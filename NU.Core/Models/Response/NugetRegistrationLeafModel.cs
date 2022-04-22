using System.Text.Json.Serialization;

namespace NU.Core.Models.Response
{
    public class NugetRegistrationLeafModel
    {
        //https://api.nuget.org/v3/registration3/nuget.server.core/3.0.0-beta.json
        [JsonPropertyName("@id")]
        public virtual string Url { get; set; }

        public NugetRegistrationCatalogEntryModel CatalogEntry { get; set; }

        //"https://api.nuget.org/v3-flatcontainer/nuget.server.core/3.0.0-beta/nuget.server.core.3.0.0-beta.nupkg",
        public string PackageContent { get; set; }

        //"https://api.nuget.org/v3/registration3/nuget.server.core/index.json"
        public string Registration { get; set; }
    }
}
