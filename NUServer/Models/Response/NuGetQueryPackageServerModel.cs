﻿using NU.Core.Models.Response;
using NUServer.Shared.Models;
using System.Text.Json.Serialization;

namespace NUServer.Models.Response
{
    public class NuGetQueryPackageServerModel : NuGetQueryPackageModel
    {
        [JsonIgnore]
        public PackageModel Data;

        public override string Id => Data.Name;

        public override string Title => Data.Name;

        public override string Description => Data.Description;

        public override string Version => Data.LatestVersion;

        public override string[] Authors => new string[] { Data.AuthorName };

        public override long TotalDownloads => Data.DownloadCount;

        public override bool Verified => true;

        public override object[] PackageTypes => new object[] { new { name = "Dependency" } };

        public NuGetQueryPackageServerModel(PackageModel data)
        {
            Data = data;

            Versions = new List<NuGetQueryPackageVersionModel>(Data.VersionList.Select(x => new NuGetQueryPackageVersionServerModel(x)));
        }
    }
}
