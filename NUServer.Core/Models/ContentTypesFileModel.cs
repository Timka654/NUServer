using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NUServer.Core.Models
{
    [XmlRoot("Types", Namespace = "http://schemas.openxmlformats.org/package/2006/content-types")]
    public class ContentTypesFileModel
    {
        [XmlElement("Default")]
        public List<ContentTypesFileDefaultModel> Types { get; set; }
    }

    public class ContentTypesFileDefaultModel
    {
        [XmlAttribute]
        public string Extension { get; set; }

        [XmlAttribute]
        public string ContentType { get; set; }
    }
}
