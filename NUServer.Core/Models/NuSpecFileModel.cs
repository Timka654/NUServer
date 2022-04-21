using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NU.Core.Models
{
    [XmlRoot("package", Namespace = "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd")]
    public class NuSpecFileModel2012 : NuSpecFileModel { }

    [XmlRoot("package", Namespace = "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")]
    public class NuSpecFileModel2013 : NuSpecFileModel { }

    public class NuSpecFileModel
    {
        [XmlNamespaceDeclarations] public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

        [XmlElement("metadata")]
        public NuSpecMetadataModel Metadata { get; set; }
    }

    public class NuSpecMetadataModel
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("version")]
        public string Version { get; set; }

        [XmlElement("authors")]
        public string Authors { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("dependencies")]
        public NuSpecDependenciesModel Dependencies { get; set; }

        [XmlElement("frameworkReferences")]
        public NuSpecFrameworkReferencesModel FrameworkReferences { get; set; }
    }

    public class NuSpecDependenciesModel
    {
        [XmlElement("group")]
        public List<NuSpecDependencyGroupModel> Groups { get; set; }
    }

    public class NuSpecFrameworkReferencesModel
    {
        [XmlElement("group")]
        public List<NuSpecFrameworkReferenceGroupModel> Groups { get; set; }
    }

    public class NuSpecFrameworkReferenceGroupModel
    {
        [XmlAttribute("targetFramework")]
        public string TargetFramework { get; set; }

        [XmlElement("dependency")]
        public List<NuSpecFrameworkReferenceModel> Dependency { get; set; }
    }

    public class NuSpecDependencyGroupModel
    {
        [XmlAttribute("targetFramework")]
        public string TargetFramework { get; set; }

        [XmlElement("dependency")]
        public List<NuSpecDependencyModel> Dependency { get; set; }
    }

    public class NuSpecDependencyModel
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlAttribute("exclude")]
        public string Exclude { get; set; }
    }

    public class NuSpecFrameworkReferenceModel
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
