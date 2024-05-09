using NU.Core.Models.Response;
using System.Text.Json.Serialization;
using NUServer.Shared.Models;

namespace NUServer.Models.Response
{
    public class NuGetQueryPackageVersionServerModel : NuGetQueryPackageVersionModel
    {
        [JsonIgnore]

        public PackageVersionModel Data;

        public override string Version => Data.Version;

        public override long Downloads => Data.DownloadCount;

        public NuGetQueryPackageVersionServerModel(PackageVersionModel data)
        {
            Data = data;
        }
    }
}
