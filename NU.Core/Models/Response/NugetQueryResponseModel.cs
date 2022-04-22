using System.Collections.Generic;

namespace NU.Core.Models.Response
{
    public class NugetQueryResponseModel
    {
        public int TotalHits { get; set; }

        public List<NugetQueryPackageModel> Data { get; set; }
    }
}
