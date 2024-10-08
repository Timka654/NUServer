using System.Collections.Generic;
using System.Xml.Serialization;

namespace NU.Core.Models
{
    [XmlRoot("Relationships", Namespace = "http://schemas.openxmlformats.org/package/2006/relationships")]
    public class RelsFileModel
    {
        [XmlNamespaceDeclarations] public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

        [XmlElement("Relationship")]
        public List<RelationshipModel> Relationships { get; set; }

        public RelsFileModel()
        {
            
        }
    }

    public class RelationshipModel
    {
        [XmlAttribute]
        public string Type { get; set; }

        [XmlAttribute]
        public string Target { get; set; }

        [XmlAttribute]
        public string Id { get; set; }

        public RelationshipModel()
        {
            
        }
    }
}
