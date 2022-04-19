using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NUServer.Models.Response
{
    public class NugetQueryPackageModel
    {
        [JsonIgnore]
        public PackageModel Data;

        public string Id => Data.Name;

        public string Title => Data.Name;

        public string Description => Data.Description;

        public string Version => Data.LatestVersion;

        public string[] Authors => new string[] { Data.AvtorName };

        public long TotalDownloads => Data.DownloadCount;

        public bool Verified => true;

        public NugetQueryPackageVersionModel[] Versions => Data.VersionList.Select(x => new NugetQueryPackageVersionModel { Data = x }).ToArray();

        public object[] PackageTypes => new object[] { new { name = "Dependency" } };
    }
}
