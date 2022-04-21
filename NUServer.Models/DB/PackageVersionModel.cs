using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUServer.Models.DB
{
    public class PackageVersionModel
    {
        public Guid PackageId { get; set; }

        public virtual PackageModel Package { get; set; }

        public string Version { get; set; }

        public DateTime UploadTime { get; set; }

        public long DownloadCount { get; set; }

        public virtual List<PackageVersionDepedencyGroupModel> DepedencyGroupList { get; set; }
    }

    public class PackageVersionDepedencyGroupModel
    { 
        public Guid Id { get; set; }


        public Guid PackageId { get; set; }

        public virtual PackageModel Package { get; set; }

        public string Version { get; set; }

        public virtual PackageVersionModel PackageVersion { get; set; }

        public string TargetFramework { get; set; }

        public List<PackageVersionDepedencyModel> Depedencies { get; set; }
    }

    public class PackageVersionDepedencyModel
    {
        public Guid Id { get; set; }

        public Guid GroupId { get; set; }

        public virtual PackageVersionDepedencyGroupModel Group { get; set; }

        public string DepedencyName { get; set; }

        public string DepedencyVersion { get; set; }

        public bool External { get; set; }
    }
}
