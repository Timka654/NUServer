using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NUServer.Core.Models
{
    [XmlRoot("Relationships", Namespace = "http://schemas.openxmlformats.org/package/2006/relationships")]
    public class RelsFileModel
    {
        [XmlElement("Relationship")]
        public List<RelationshipModel> Relationships { get; set; }
    }

    public class RelationshipModel
    {
        [XmlAttribute]
        public string Type { get; set; }

        [XmlAttribute]
        public string Target { get; set; }

        [XmlAttribute]
        public string Id { get; set; }
    }
}
