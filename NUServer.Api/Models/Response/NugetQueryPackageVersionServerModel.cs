using NUServer.Models;
using NU.Core.Models.Response;
using System.Text.Json.Serialization;

namespace NUServer.Api.Models.Response
{
    public class NugetQueryPackageVersionServerModel : NugetQueryPackageVersionModel
    {
        [JsonIgnore]

        public PackageVersionModel Data;

        public override string Version => Data.Version;

        public override long Downloads => Data.DownloadCount;

        public NugetQueryPackageVersionServerModel(PackageVersionModel data)
        {
            Data = data;
        }
    }
}
