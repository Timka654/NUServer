using NUServer.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NUServer.Core
{
    public class NugetFile : IDisposable
    {
        private const string relsFilePath = "_rels/.rels";

        private const string contentTypesFilePath = "[Content_Types].xml";

        private const string corePropertiesRelPath = "package/services/metadata/core-properties";

        private static readonly Dictionary<string, string> TypeExtensionMap = new()
        {
            { "nuspec", "http://schemas.microsoft.com/packaging/2010/07/manifest" },
            { "psmdcp", "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties" },
        };

        private ZipArchive nugetFile;

        private NuSpecFileModel nuspecFile;

        private RelsFileModel relsFile;

        private PsmdcpFileModel PsmdcpFile;

        private ContentTypesFileModel ContentTypesFile;

        private NugetFile(ZipArchive archive)
        {
            nugetFile = archive;

            ReadContentTypesFile();

            ReadRelsFile();

            ReadNUSPECFile();

            ReadPsmdcpFile();
        }

        public NugetFile(string packageName, string version, string authors)
        {
            ContentTypesFile = new ContentTypesFileModel()
            {
                Types = new List<ContentTypesFileDefaultModel>()
                {
                new ContentTypesFileDefaultModel(){ Extension = "rels", ContentType = "application/vnd.openxmlformats-package.relationships+xml" },
                new ContentTypesFileDefaultModel(){ Extension = "psmdcp", ContentType = "application/vnd.openxmlformats-package.core-properties+xml" },
                new ContentTypesFileDefaultModel(){ Extension = "dll", ContentType = "application/octet" },
                new ContentTypesFileDefaultModel(){ Extension = "nuspec", ContentType = "application/octet" }
                }
            };

            nuspecFile = new NuSpecFileModel2013()
            {
                Metadata = new NuSpecMetadataModel()
                {
                    Dependencies = new NuSpecDependenciesModel()
                    {
                        Groups = new List<NuSpecDependencyGroupModel>()
                    }
                }
            };

            relsFile = new RelsFileModel() { Relationships = new List<RelationshipModel>() };

            PsmdcpFile = new PsmdcpFileModel();

            Id = packageName;

            Version = version;

            Authors = authors;
        }

        #region Write

        public void CreatePackage(string path)
        {
            path = Path.Combine(path, $"{Id}.{Version}.nupkg");

            //debug

            if (Directory.Exists(path))
                Directory.Delete(path, true);

            Directory.CreateDirectory(path);

            //debug

            WriteContentTypesFile(path);

            WriteRelsFile(path);

            WriteNUSPECFile(path);

            WritePsmdcpFile(path);
        }

        private void WriteRelsFile(string path)
        {
            var relsPath = Path.Combine(path, "_rels");

            Directory.CreateDirectory(relsPath);

            relsPath = Path.Combine(path, relsFilePath);


            relsFile.Relationships.Clear();

            relsFile.Relationships.Add(new RelationshipModel() { Id = "??", Target = $"/{Id}.nuspec", Type = TypeExtensionMap["nuspec"] });
            relsFile.Relationships.Add(new RelationshipModel() { Id = "??", Target = $"/{corePropertiesRelPath}/??.psmdcp", Type = TypeExtensionMap["psmdcp"] });


            XmlSerializer xs = new XmlSerializer(typeof(RelsFileModel));

            using (var file = File.Create(relsPath))
            {
                using (var xmlWriter = XmlWriter.Create(file, new XmlWriterSettings { Indent = true }))
                    xs.Serialize(xmlWriter, relsFile);
            }
        }

        public void WriteNUSPECFile(string dir)
        {
            var nuspecPath = Path.Combine(dir, $"{Id}.nuspec");

            using (var file = File.Create(nuspecPath))
            {
                WriteNUSPECFile(file);
            }
        }

        public void WriteNUSPECFile(Stream stream)
        {
            if(nuspecFile == null)
                throw new ArgumentNullException(nameof(nuspecFile));

            using (stream)
            {
                using (var xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }))
                {
                    if (nuspecFile is NuSpecFileModel2012 nuSpec2012)
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(NuSpecFileModel2012));
                        xs.Serialize(xmlWriter, nuSpec2012);
                    }
                    else if (nuspecFile is NuSpecFileModel2013 nuSpec2013)
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(NuSpecFileModel2013));
                        xs.Serialize(xmlWriter, nuSpec2013);
                    }
                    else
                    {
                        throw new InvalidCastException($"nuspecFile have unsupported type {nuspecFile?.GetType()}");
                    }
                }
            }
        }

        private void WritePsmdcpFile(string path)
        {
            var nuspecPath = Path.Combine(path, corePropertiesRelPath, $"??.psmdcp");

            XmlSerializer xs = new XmlSerializer(typeof(PsmdcpFileModel));

            using (var file = File.Create(nuspecPath))
            {
                using (var xmlWriter = XmlWriter.Create(file, new XmlWriterSettings { Indent = true }))
                    xs.Serialize(xmlWriter, PsmdcpFile);
            }
        }

        private void WriteContentTypesFile(string path)
        {
            var contentTypesPath = Path.Combine(path, contentTypesFilePath);

            XmlSerializer xs = new XmlSerializer(typeof(ContentTypesFileModel));

            using (var file = File.Create(contentTypesPath))
            {
                using (var xmlWriter = XmlWriter.Create(file, new XmlWriterSettings { Indent = true }))
                    xs.Serialize(xmlWriter, ContentTypesFile);
            }
        }

        #endregion

        #region PackageInfo

        public string Id
        {
            get => nuspecFile.Metadata.Id;
            set => PsmdcpFile.Identifier = nuspecFile.Metadata.Id = value;
        }

        public string Version
        {
            get => nuspecFile.Metadata.Version;
            set => PsmdcpFile.Version = nuspecFile.Metadata.Version = value;
        }

        public string Authors
        {
            get => nuspecFile.Metadata.Authors;
            set => PsmdcpFile.Creator = nuspecFile.Metadata.Authors = value;
        }

        public string Description
        {
            get => nuspecFile.Metadata.Description;
            set => PsmdcpFile.Description = nuspecFile.Metadata.Description = value;
        }

        public string Keywords
        {
            get => PsmdcpFile.Keywords;
            set => PsmdcpFile.Keywords = value;
        }

        public string LastModifiedBy
        {
            get => PsmdcpFile.LastModifiedBy;
            set => PsmdcpFile.LastModifiedBy = value;
        }

        public NuSpecDependenciesModel Dependencies => nuspecFile.Metadata.Dependencies;

        #endregion

        #region Read

        private void ReadRelsFile()
        {
            var fileEntry = nugetFile.GetEntry(relsFilePath);

            using (var stream = fileEntry.Open())
            {
                var xs = new XmlSerializer(typeof(RelsFileModel));

                relsFile = xs.Deserialize(stream) as RelsFileModel;
            }
        }

        private void ReadNUSPECFile()
        {
            var nuspecRel = relsFile.Relationships.First(x => x.Type == TypeExtensionMap["nuspec"]);

            var fileEntry = nugetFile.GetEntry(normalizeRelPath(nuspecRel.Target));

            var xDocument = XDocument.Load(fileEntry.Open());

            string @namespace = xDocument.Root?.Name.Namespace.NamespaceName;

            using (var stream = xDocument.Root.CreateReader())
            {
                XmlSerializer xs = default;

                if (@namespace == "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")
                {
                    xs = new XmlSerializer(typeof(NuSpecFileModel2013));

                    nuspecFile = xs.Deserialize(stream) as NuSpecFileModel2013;
                }
                else if (@namespace == "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd")
                {
                    xs = new XmlSerializer(typeof(NuSpecFileModel2012));

                    nuspecFile = xs.Deserialize(stream) as NuSpecFileModel2012;
                }
                else
                {
                    throw new InvalidCastException($"NUSpec file have unsupported namespace {xDocument.Root?.Name.Namespace.NamespaceName}");
                }
            }
        }

        private void ReadPsmdcpFile()
        {
            var nuspecRel = relsFile.Relationships.First(x => x.Type == TypeExtensionMap["psmdcp"]);

            var fileEntry = nugetFile.GetEntry(normalizeRelPath(nuspecRel.Target));

            using (var stream = fileEntry.Open())
            {
                var xs = new XmlSerializer(typeof(PsmdcpFileModel));

                PsmdcpFile = xs.Deserialize(stream) as PsmdcpFileModel;
            }
        }

        private void ReadContentTypesFile()
        {
            var fileEntry = nugetFile.GetEntry(contentTypesFilePath);

            using (var stream = fileEntry.Open())
            {
                var xs = new XmlSerializer(typeof(ContentTypesFileModel));

                ContentTypesFile = xs.Deserialize(stream) as ContentTypesFileModel;
            }
        }

        private string normalizeRelPath(string path) => path.TrimStart('/');

        public static NugetFile Read(string path) => Read(File.OpenRead(path));

        public static NugetFile Read(byte[] data)
            => Read(new MemoryStream(data));

        public static NugetFile Read(Stream path)
            => new NugetFile(new ZipArchive(path));

        #endregion

        public void Dispose()
        {
            nugetFile?.Dispose();
        }
    }

    // This class copied from this answer https://stackoverflow.com/a/873281/3744182
    // To https://stackoverflow.com/questions/870293/can-i-make-xmlserializer-ignore-the-namespace-on-deserialization
    // By https://stackoverflow.com/users/48082/cheeso
    // helper class to ignore namespaces when de-serializing
    public class NamespaceIgnorantXmlTextReader : XmlTextReader
    {
        public NamespaceIgnorantXmlTextReader(System.IO.Stream reader) : base(reader) { }

        public override string NamespaceURI { get { return ""; } }
    }
}
