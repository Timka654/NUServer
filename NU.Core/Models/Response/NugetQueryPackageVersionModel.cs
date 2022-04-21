using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NU.Core.Models.Response
{
    public class NugetQueryPackageVersionModel
    {
        public virtual string Version { get; set; }

        public virtual long Downloads { get; set; }
    }
}
