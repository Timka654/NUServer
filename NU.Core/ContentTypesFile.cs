using NU.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NU.Core
{
    public class ContentTypesFile
    {
        public ContentTypesFileModel Data { get; private set; }

        public ContentTypesFile()
        {
            Data = new ContentTypesFileModel()
            {
                Types = new List<ContentTypesFileDefaultModel>()
                {
                new ContentTypesFileDefaultModel(){ Extension = "rels", ContentType = "application/vnd.openxmlformats-package.relationships+xml" },
                new ContentTypesFileDefaultModel(){ Extension = "psmdcp", ContentType = "application/vnd.openxmlformats-package.core-properties+xml" },
                new ContentTypesFileDefaultModel(){ Extension = "dll", ContentType = "application/octet" },
                new ContentTypesFileDefaultModel(){ Extension = "nuspec", ContentType = "application/octet" }
                }
            };
        }

        public ContentTypesFile(Stream stream)
        {
            Read(stream);
        }

        private void Read(Stream stream)
        {
            var xs = new XmlSerializer(typeof(ContentTypesFileModel));

            Data = xs.Deserialize(stream) as ContentTypesFileModel;
        }

        public void Write(string dir)
        {
            var contentTypesPath = Path.Combine(dir, NugetFile.ContentTypesFilePath);

            using (var file = File.Create(contentTypesPath))
                Write(file);
        }

        public void Write(ZipArchive archive)
        {
            var entry = archive.CreateEntry(NugetFile.ContentTypesFilePath);

            using (var file = entry.Open())
                Write(file);
        }

        public void Write(Stream stream)
        {
            XmlSerializerNamespaces myNamespaces = new XmlSerializerNamespaces();
            myNamespaces.Add("", "");

            XmlSerializer xs = new XmlSerializer(typeof(ContentTypesFileModel));

            using (var xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }))
                xs.Serialize(xmlWriter, Data, myNamespaces);
        }
    }
}
