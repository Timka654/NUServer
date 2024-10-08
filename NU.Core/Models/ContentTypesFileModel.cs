using System.Collections.Generic;
using System.Xml.Serialization;

namespace NU.Core.Models
{
    [XmlRoot("Types", Namespace = "http://schemas.openxmlformats.org/package/2006/content-types")]
    public class ContentTypesFileModel
    {
        [XmlElement("Default")]
        public List<ContentTypesFileDefaultModel> Types { get; set; }

        public ContentTypesFileModel()
        {
                
        }
    }

    public class ContentTypesFileDefaultModel
    {
        [XmlAttribute]
        public string Extension { get; set; }

        [XmlAttribute]
        public string ContentType { get; set; }

        public ContentTypesFileDefaultModel()
        {
                
        }
    }
}
