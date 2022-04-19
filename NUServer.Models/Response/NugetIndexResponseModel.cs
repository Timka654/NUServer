using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUServer.Models.Response
{
    public class NugetIndexResponseModel
    {
        public string Version { get; set; }

        public ResourceModel[] Resources { get; set; }
    }
}
