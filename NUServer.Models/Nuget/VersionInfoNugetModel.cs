using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NUServer.Models.Nuget
{
    public class VersionInfoNugetModel
    {
        [JsonPropertyName("@id")]
        public string Id { get; set; } //"https://api.nuget.org/v3/registration5-gz-semver2/newtonsoft.json/3.5.8.json",

        [JsonPropertyName("@type")]
        public string[] Type => new string[] { "Package", "http://schema.nuget.org/catalog#Permalink" };

        public string CatalogEntry { get; set; } //"https://api.nuget.org/v3/catalog0/data/2018.10.15.01.11.37/newtonsoft.json.3.5.8.json",

        public bool Listed { get; set; } = true; // true

        public string PackageContent { get; set; } //"https://api.nuget.org/v3-flatcontainer/newtonsoft.json/3.5.8/newtonsoft.json.3.5.8.nupkg",

        public DateTime Published { get; set; } //"2011-01-08T22:12:57.713+00:00",

        public string Registration { get; set; } //"https://api.nuget.org/v3/registration5-gz-semver2/newtonsoft.json/index.json",

        [JsonPropertyName("@context")]
        public ContextModel Context { get; set; } = new ContextModel();

        public class ContextModel
        {
            [JsonPropertyName("@vocab")]
            public string Vocab { get; set; }

            public string Xsd { get; set; }

            public EntryModel CatalogEntry { get; set; } = new EntryModel() { Type = "@id" };

            public EntryModel Registration { get; set; } = new EntryModel() { Type = "@id" };

            public EntryModel PackageContent { get; set; } = new EntryModel() { Type = "@id" };

            public EntryModel Published { get; set; } = new EntryModel() { Type = "xsd:dateTime" };

            public class EntryModel
            {
                [JsonPropertyName("@type")]
                public string Type { get; set; }
            }
        }
    }
}
