using System.Collections.Generic;
using System.Xml.Serialization;

namespace NU.Core.Models
{
    [XmlRoot("package", Namespace = "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd")]
    public class NuSpecFileModel2010 : NuSpecFileModel {
        public NuSpecFileModel2010()
        {
            
        }
    }

    [XmlRoot("package", Namespace = "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd")]
    public class NuSpecFileModel2012 : NuSpecFileModel {
        public NuSpecFileModel2012()
        {
            
        }
    }

    [XmlRoot("package", Namespace = "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")]
    public class NuSpecFileModel2013 : NuSpecFileModel {
        public NuSpecFileModel2013()
        {
            
        }
    }

    public class NuSpecFileModel
    {
        [XmlNamespaceDeclarations] public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

        [XmlElement("metadata")]
        public NuSpecMetadataModel Metadata { get; set; }

        public NuSpecFileModel()
        {
            
        }
    }

    public class NuSpecMetadataModel
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("version")]
        public string Version { get; set; }

        [XmlElement("authors")]
        public string Authors { get; set; }

        [XmlElement("owners")]
        public string Owners { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("requireLicenseAcceptance")]
        public bool RequireLicenseAcceptance { get; set; }

        [XmlElement("icon")]
        public string Icon { get; set; }

        [XmlElement("tags")]
        public string Tags { get; set; }

        [XmlElement("projectUrl")]
        public string ProjectUrl { get; set; }

        [XmlElement("copyright")]
        public string Copyright { get; set; }

        [XmlElement("readme")]
        public string Readme { get; set; }

        [XmlElement("releaseNotes")]
        public string ReleaseNotes { get; set; }

        [XmlElement("summary")]
        public string Summary { get; set; }

        [XmlElement("licenseUrl")]
        public string LicenseUrl { get; set; }

        [XmlElement("language")]
        public string Language { get; set; }

        [XmlElement("dependencies")]
        public NuSpecDependenciesModel Dependencies { get; set; }

        [XmlElement("frameworkReferences")]
        public NuSpecFrameworkReferencesModel FrameworkReferences { get; set; }

        public NuSpecMetadataModel()
        {
            
        }
    }

    public class NuSpecDependenciesModel
    {
        [XmlElement("group")]
        public List<NuSpecDependencyGroupModel> Groups { get; set; }

        public NuSpecDependenciesModel()
        {
            
        }
    }

    public class NuSpecFrameworkReferencesModel
    {
        [XmlElement("group")]
        public List<NuSpecFrameworkReferenceGroupModel> Groups { get; set; }

        public NuSpecFrameworkReferencesModel()
        {
            
        }
    }

    public class NuSpecFrameworkReferenceGroupModel
    {
        [XmlAttribute("targetFramework")]
        public string TargetFramework { get; set; }

        [XmlElement("dependency")]
        public List<NuSpecFrameworkReferenceModel> Dependency { get; set; }

        public NuSpecFrameworkReferenceGroupModel()
        {
            
        }
    }

    public class NuSpecDependencyGroupModel
    {
        [XmlAttribute("targetFramework")]
        public string TargetFramework { get; set; }

        [XmlElement("dependency")]
        public List<NuSpecDependencyModel> Dependency { get; set; }

        public NuSpecDependencyGroupModel()
        {
            
        }
    }

    public class NuSpecDependencyModel
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlAttribute("exclude")]
        public string Exclude { get; set; }

        public NuSpecDependencyModel()
        {
            
        }
    }

    public class NuSpecFrameworkReferenceModel
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public NuSpecFrameworkReferenceModel()
        {
            
        }
    }
}
