using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NUServer.Models.Response
{
    public class NugetQueryPackageVersionModel
    {
        [JsonIgnore]
        public PackageVersionModel Data;

        public string Version => Data.Version;

        public long Downloads => Data.DownloadCount;
    }
}
