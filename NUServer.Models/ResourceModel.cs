using System.Text.Json.Serialization;

namespace NUServer.Models
{
    public class ResourceModel
    {
        [JsonPropertyName("@id")]
        public string Url { get; set; }

        [JsonPropertyName("@type")]
        public string Type { get; set; }

        public string Comment { get; set; }
    }
}