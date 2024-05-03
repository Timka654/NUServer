using System.Text.Json.Serialization;

namespace NU.Core.Models.Response
{
    public class NuGetRegistrationLeafModel
    {
        //https://api.NuGet.org/v3/registration3/NuGet.server.core/3.0.0-beta.json
        [JsonPropertyName("@id")]
        public virtual string Url { get; set; }

        public NuGetRegistrationCatalogEntryModel CatalogEntry { get; set; }

        //"https://api.NuGet.org/v3-flatcontainer/NuGet.server.core/3.0.0-beta/NuGet.server.core.3.0.0-beta.nupkg",
        public string PackageContent { get; set; }

        //"https://api.NuGet.org/v3/registration3/NuGet.server.core/index.json"
        public string Registration { get; set; }
    }
}
