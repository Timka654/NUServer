using NU.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace NU.Core
{
    public class PsmdcpFile
    {
        public PsmdcpFileModel Data { get; set; }

        public PsmdcpFile()
        {
            Data = new PsmdcpFileModel();
        }

        public PsmdcpFile(Guid psmdcpName) : this()
        {
            this.PsmdcpName = psmdcpName;
        }

        public PsmdcpFile(Stream stream)
        {
            Read(stream);
        }

        public PsmdcpFile(Stream stream, Guid psmdcpName) : this(stream)
        {
            this.PsmdcpName = psmdcpName;
        }

        private void Read(Stream stream)
        {
            var xs = new XmlSerializer(typeof(PsmdcpFileModel));

            Data = xs.Deserialize(stream) as PsmdcpFileModel;
        }

        public void Write(string path)
        {
            var nuspecPath = Path.Combine(path, NugetFile.CorePropertiesRelPath);

            if (!Directory.Exists(nuspecPath))
                Directory.CreateDirectory(nuspecPath);

            nuspecPath = Path.Combine(nuspecPath, $"{CalcPsmdcpName()}.psmdcp");

            using (var file = File.Create(nuspecPath))
                Write(file);
        }

        internal void Write(ZipArchive nugetFile)
        {
            var nuspecPath = Path.Combine(NugetFile.CorePropertiesRelPath, $"{CalcPsmdcpName()}.psmdcp");

            var entry = nugetFile.CreateEntry(nuspecPath);

            entry.LastWriteTime = DateTime.UtcNow;

            using (var file = entry.Open())
                Write(file);
        }

        public void Write(Stream stream)
        {
            XmlSerializerNamespaces myNamespaces = new XmlSerializerNamespaces();
            myNamespaces.Add("", "");
            myNamespaces.Add("dc", "http://purl.org/dc/elements/1.1/");
            myNamespaces.Add("dcterms", "http://purl.org/dc/terms/");
            myNamespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            //dc="http://purl.org/dc/elements/1.1/" dcterms="http://purl.org/dc/terms/" xsi="http://www.w3.org/2001/XMLSchema-instance"

            Data.LastModifiedBy = CreatorInfo();

            XmlSerializer xs = new XmlSerializer(typeof(PsmdcpFileModel));

            using (var xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, WriteEndDocumentOnClose = true }))
                xs.Serialize(xmlWriter, Data, myNamespaces);
        }

        private Guid PsmdcpName = default;

        public string CalcPsmdcpName()
        {
            if (PsmdcpName == default)
                PsmdcpName = Guid.NewGuid();

            return PsmdcpName.ToString("N");
        }

        private static string CreatorInfo()
        {
            List<string> creatorInfo = new List<string>();
            var assembly = typeof(NugetFile).GetTypeInfo().Assembly;
            creatorInfo.Add(assembly.FullName);
#if !IS_CORECLR // CORECLR_TODO: Environment.OSVersion
            creatorInfo.Add(Environment.OSVersion.ToString());
#endif

            var attribute = assembly.GetCustomAttributes<System.Runtime.Versioning.TargetFrameworkAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                creatorInfo.Add(attribute.FrameworkDisplayName);
            }

            return string.Join(";", creatorInfo);
        }
    }
}
