using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUServer.Shared.Models.Request
{
    public class RemovePackageVersionRequestModel
    {
        public Guid PackageId { get; set; }

        public string PackageVersion { get; set; }
    }
}
