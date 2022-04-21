using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NU.Core.Models.Response
{
    public class NugetRegistrationCatalogVulnerabilitiesModel
    {
        //"https://github.com/advisories/ABCD-1234-5678-9012"
        public string AdvisoryUrl { get; set; }

        //"2"
        public string Severity { get; set; }
    }
}
