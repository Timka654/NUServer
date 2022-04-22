using NU.Core.Models.Response;
using NUServer.Models.DB;
using System.Text.Json.Serialization;

namespace NUServer.Api.Models.Response
{
    public class NugetQueryPackageServerModel : NugetQueryPackageModel
    {
        [JsonIgnore]
        public PackageModel Data;

        public override string Id => Data.Name;

        public override string Title => Data.Name;

        public override string Description => Data.Description;

        public override string Version => Data.LatestVersion;

        public override string[] Authors => new string[] { Data.AvtorName };

        public override long TotalDownloads => Data.DownloadCount;

        public override bool Verified => true;

        public override object[] PackageTypes => new object[] { new { name = "Dependency" } };

        public NugetQueryPackageServerModel(PackageModel data)
        {
            Data = data;

            Versions = new List<NugetQueryPackageVersionModel>(Data.VersionList.Select(x => new NugetQueryPackageVersionServerModel(x)));
        }
    }
}
