using NU.Core.Models;
using NU.Core.Utils;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace NU.Core
{
    public class RelsFile
    {
        internal static readonly Dictionary<string, string> TypeExtensionMap = new Dictionary<string, string>()
        {
            { "nuspec", "http://schemas.microsoft.com/packaging/2010/07/manifest" },
            { "psmdcp", "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties" },
        };

        public RelsFileModel Data { get; private set; }

        public RelsFile()
        {
            Data = new RelsFileModel() { Relationships = new List<RelationshipModel>() };
        }

        public RelsFile(Stream stream)
        {
            Read(stream);
        }

        private void Read(Stream stream)
        {
            var xs = new XmlSerializer(typeof(RelsFileModel));

            Data = xs.Deserialize(stream) as RelsFileModel;
        }

        public void Write(string id, PsmdcpFile psmdcp, string dir)
        {
            var filePath = new FileInfo(Path.Combine(dir, NugetFile.RelsFilePath));

            if (!filePath.Directory.Exists)
                filePath.Directory.Create();

            using (var stream = File.OpenWrite(filePath.FullName))
                Write(id, psmdcp, stream);
        }

        public void Write(string id, PsmdcpFile psmdcp, ZipArchive nugetFile)
        {
            var entry = nugetFile.CreateEntry(NugetFile.RelsFilePath);

            using (var file = entry.Open())
                Write(id, psmdcp, file);
        }

        public void Write(string id, PsmdcpFile psmdcp, Stream stream)
        {
            Data.Relationships.Clear();

            Data.Relationships.Add(CreateRelationship($"/{id}.nuspec", TypeExtensionMap["nuspec"]));
            Data.Relationships.Add(CreateRelationship($"/{NugetFile.CorePropertiesRelPath}/{psmdcp.CalcPsmdcpName()}.psmdcp", TypeExtensionMap["psmdcp"]));

            XmlSerializerNamespaces myNamespaces = Data.xmlns;

            if (!myNamespaces.ToArray().Any(x => x.Name == string.Empty))
                myNamespaces.Add("", "");

            XmlSerializer xs = new XmlSerializer(typeof(RelsFileModel));

            using (var xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }))
                xs.Serialize(xmlWriter, Data, myNamespaces);
        }

        private RelationshipModel CreateRelationship(string target, string type)
            => new RelationshipModel() { Id = GenerateRelationshipId(target), Target = target, Type = type };


        // Generate a relationship id for compatibility
        public static string GenerateRelationshipId(string path)
        {
            using (var hashFunc = new Sha512HashFunction())
            {
                var data = System.Text.Encoding.UTF8.GetBytes(path);
                hashFunc.Update(data, 0, data.Length);
                var hash = hashFunc.GetHashBytes();
                var hex = Hex.EncodeHexString(hash);
                return "R" + hex.Substring(0, 16);
            }
        }
    }
}
