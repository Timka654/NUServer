using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NU.Core.Models.Response
{
    public class NugetRegistrationPageModel
    {
        //"https://api.nuget.org/v3/registration3/nuget.server.core/index.json#page/3.0.0-beta/3.0.0-beta",
        [JsonPropertyName("@id")]
        public virtual string Url { get; set; }

        public int Count => Items.Count;

        public List<NugetRegistrationLeafModel> Items { get; set; }

        //"3.0.0-beta",
        public string Lower { get; set; }

        //"3.0.0-beta"
        public string Upper { get; set; }
    }
}
