using NU.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NU.Core
{
    public class NuSpecFile
    {
        public NuSpecFileModel Data { get; private set; }

        public NuSpecFile()
        {
            Data = new NuSpecFileModel2013()
            {
                Metadata = new NuSpecMetadataModel()
                {
                    Dependencies = new NuSpecDependenciesModel()
                    {
                        Groups = new List<NuSpecDependencyGroupModel>()
                    }
                }
            };
        }

        public NuSpecFile(Stream stream)
        {
            Read(stream);
        }

        private void Read(Stream fileEntry)
        {
            var xDocument = XDocument.Load(fileEntry);

            string @namespace = null;

            if (xDocument.Root != null)
                @namespace = xDocument.Root.Name.Namespace.NamespaceName;

            using (var stream = xDocument.Root.CreateReader())
            {
                XmlSerializer xs = default;

                if (@namespace == "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")
                {
                    xs = new XmlSerializer(typeof(NuSpecFileModel2013));

                    Data = xs.Deserialize(stream) as NuSpecFileModel2013;
                }
                else if (@namespace == "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd")
                {
                    xs = new XmlSerializer(typeof(NuSpecFileModel2012));

                    Data = xs.Deserialize(stream) as NuSpecFileModel2012;
                }
                else
                {
                    throw new InvalidCastException($"NUSpec file have unsupported namespace {@namespace}");
                }
            }
        }

        public void Write(string id, string dir)
        {
            var nuspecPath = Path.Combine(dir, $"{id}.nuspec");

            using (var file = File.Create(nuspecPath))
            {
                Write(file);
            }
        }

        public void Write(string id, ZipArchive nugetFile)
        {
            var entry = nugetFile.CreateEntry($"{id}.nuspec");

            using (var file = entry.Open())
                Write(file);
        }

        public void Write(Stream stream)
        {
            if (Data == null)
                throw new ArgumentNullException(nameof(Data));

            XmlSerializerNamespaces myNamespaces = new XmlSerializerNamespaces();
            myNamespaces.Add("", "");

            using (var xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }))
            {
                if (Data is NuSpecFileModel2012 nuSpec2012)
                {
                    XmlSerializer xs = new XmlSerializer(typeof(NuSpecFileModel2012));
                    xs.Serialize(xmlWriter, nuSpec2012, myNamespaces);
                }
                else if (Data is NuSpecFileModel2013 nuSpec2013)
                {
                    XmlSerializer xs = new XmlSerializer(typeof(NuSpecFileModel2013));
                    xs.Serialize(xmlWriter, nuSpec2013, myNamespaces);
                }
                else
                {
                    throw new InvalidCastException($"nuspecFile have unsupported type {Data.GetType()}");
                }
            }
        }
    }
}
