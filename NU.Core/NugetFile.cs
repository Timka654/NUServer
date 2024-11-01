﻿using NU.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace NU.Core
{
    public class NuGetFile : IDisposable
    {
        internal const string RelsFilePath = "_rels/.rels";

        internal const string ContentTypesFilePath = "[Content_Types].xml";

        internal const string CorePropertiesRelPath = "package/services/metadata/core-properties";

        private ZipArchive nugetFile;

        public NuSpecFile NUSpecFile { get; protected set; }

        public RelsFile RelsFile { get; protected set; }

        public PsmdcpFile PsmdcpFile { get; protected set; }

        public ContentTypesFile ContentTypesFile { get; protected set; }


        public NuGetFile(string path) : this(File.OpenRead(path)) { }

        public NuGetFile(byte[] data) : this(new MemoryStream(data)) { }

        public NuGetFile(Stream path) : this(new ZipArchive(path)) { }

        private NuGetFile(ZipArchive archive)
        {
            nugetFile = archive;

            ReadContentTypesFile();

            ReadRelsFile();

            ReadNUSPECFile();

            ReadPsmdcpFile();
        }

        public NuGetFile(string packageName, string version, string authors)
        {
            NUSpecFile = new NuSpecFile();

            RelsFile = new RelsFile();

            PsmdcpFile = new PsmdcpFile();

            Id = packageName;

            Version = version;

            Authors = authors;
        }

        #region Write

        public void CreatePackageDirectory(string dir)
        {
            dir = Path.Combine(dir, $"{Id}.{Version}.nupkg");

            if (Directory.Exists(dir))
                Directory.Delete(dir, true);

            Directory.CreateDirectory(dir);


            ContentTypesFile.Write(dir);

            RelsFile.Write(Id, PsmdcpFile, dir);

            NUSpecFile.Write(Id, dir);

            PsmdcpFile.Write(dir);

        }

        public void CreatePackage(string fileName)
        {
            using (var stream = File.OpenWrite(fileName))
                CreatePackage(stream);
        }

        public void CreatePackage(Stream stream)
        {
            using (nugetFile = new ZipArchive(stream, ZipArchiveMode.Create))
            {
                ContentTypesFile.Write(nugetFile);

                RelsFile.Write(Id, PsmdcpFile, nugetFile);

                NUSpecFile.Write(Id, nugetFile);

                PsmdcpFile.Write(nugetFile);
            }
        }

        #endregion

        #region PackageInfo

        public string Id
        {
            get => NUSpecFile.Data.Metadata.Id;
            set => PsmdcpFile.Data.Identifier = NUSpecFile.Data.Metadata.Id = value;
        }

        public string Version
        {
            get => NUSpecFile.Data.Metadata.Version;
            set => PsmdcpFile.Data.Version = NUSpecFile.Data.Metadata.Version = value;
        }

        public string Authors
        {
            get => NUSpecFile.Data.Metadata.Authors;
            set => PsmdcpFile.Data.Creator = NUSpecFile.Data.Metadata.Authors = value;
        }

        public string Description
        {
            get => NUSpecFile.Data.Metadata.Description;
            set => PsmdcpFile.Data.Description = NUSpecFile.Data.Metadata.Description = value;
        }

        public string Keywords
        {
            get => PsmdcpFile.Data.Keywords;
            set => PsmdcpFile.Data.Keywords = value;
        }

        public string LastModifiedBy
        {
            get => PsmdcpFile.Data.LastModifiedBy;
            set => PsmdcpFile.Data.LastModifiedBy = value;
        }

        public NuSpecDependenciesModel Dependencies => NUSpecFile.Data.Metadata.Dependencies;

        #endregion

        #region Read

        public IReadOnlyCollection<ZipArchiveEntry> GetEntries()
        {
            return nugetFile.Entries;
        }

        public ZipArchiveEntry[] GetFiles(string targetFramework)
        {
            return nugetFile.Entries.Where(x => x.FullName.StartsWith($"lib/{targetFramework}/", StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public void DumpFrameworkFiles(string dir, string targetFramework)
        {
            if (!Directory.Exists(dir))
                throw new DirectoryNotFoundException(dir);

            var entityes = GetFiles(targetFramework);

            if (entityes.Any() == false)
                throw new ArgumentOutOfRangeException(targetFramework);

            foreach (var item in entityes)
            {
                item.ExtractToFile(Path.Combine(dir, item.Name), true);
            }
        }

        private void ReadContentTypesFile()
        {
            var fileEntry = nugetFile.GetEntry(ContentTypesFilePath);

            using (var stream = fileEntry.Open())
                ContentTypesFile = new ContentTypesFile(stream);
        }

        private void ReadRelsFile()
        {
            var fileEntry = nugetFile.GetEntry(RelsFilePath);

            using (var stream = fileEntry.Open())
                RelsFile = new RelsFile(stream);
        }

        private void ReadNUSPECFile()
        {
            var nuspecRel = RelsFile.Data.Relationships.First(x => x.Type == RelsFile.TypeExtensionMap["nuspec"]);

            var fileEntry = nugetFile.GetEntry(new string(nuspecRel.Target.Skip(1).ToArray()));

            using (var stream = fileEntry.Open())
                NUSpecFile = new NuSpecFile(stream);
        }

        private void ReadPsmdcpFile()
        {
            var nuspecRel = RelsFile.Data.Relationships.First(x => x.Type == RelsFile.TypeExtensionMap["psmdcp"]);

            var fileEntry = nugetFile.GetEntry(new string(nuspecRel.Target.Skip(1).ToArray()));

            if (!Guid.TryParse(Path.GetFileName(nuspecRel.Target).Split('.').First(), out var psmdcpName))
                throw new InvalidDataException(nuspecRel.Target);

            using (var stream = fileEntry.Open())
                PsmdcpFile = new PsmdcpFile(stream, psmdcpName);
        }


        #endregion

        public void Dispose()
        {
            if (nugetFile != null)
                nugetFile.Dispose();
        }
    }
}
