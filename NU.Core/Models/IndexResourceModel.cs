using System.Text.Json.Serialization;

namespace NU.Core.Models
{
    public class IndexResourceModel
    {
        [JsonPropertyName("@id")]
        public virtual string Url { get; set; }

        [JsonPropertyName("@type")]
        public virtual string Type { get; set; }

        public virtual string Comment { get; set; }

        public IndexResourceModel()
        {
                
        }
    }
}