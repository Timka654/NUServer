using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NUServer.Core.Models
{
    [XmlRoot("coreProperties", Namespace = "http://schemas.openxmlformats.org/package/2006/metadata/core-properties")]
    public class PsmdcpFileModel
    {
        [XmlElement("creator", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Creator { get; set; }

        [XmlElement("description", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Description { get; set; }

        [XmlElement("identifier", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Identifier { get; set; }

        [XmlElement("version")]
        public string Version { get; set; }

        [XmlElement("keywords")]
        public string Keywords { get; set; }

        [XmlElement("lastModifiedBy")]
        public string LastModifiedBy { get; set; }
    }
}
