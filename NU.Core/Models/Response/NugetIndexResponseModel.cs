using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NU.Core.Models.Response
{
    public class NugetIndexResponseModel
    {
        public string Version { get; set; }

        public IndexResourceModel[] Resources { get; set; }
    }
}
