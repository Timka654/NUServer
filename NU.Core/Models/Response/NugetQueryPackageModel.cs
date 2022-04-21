using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NU.Core.Models.Response
{
    public class NugetQueryPackageModel
    {
        public virtual string Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual string Version { get; set; }

        public virtual string[] Authors { get; set; }

        public virtual long TotalDownloads { get; set; }

        public virtual bool Verified { get; set; }

        public virtual NugetQueryPackageVersionModel[] Versions { get; set; }

        public virtual object[] PackageTypes { get; set; }
    }
}
