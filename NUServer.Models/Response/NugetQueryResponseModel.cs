using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUServer.Models.Response
{
    public class NugetQueryResponseModel
    {
        public int TotalHits => Data.Length;

        public NugetQueryPackageModel[] Data { get; set; }
    }
}
