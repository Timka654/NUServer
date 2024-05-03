using System.Collections.Generic;

namespace NU.Core.Models.Response
{
    public class NuGetQueryResponseModel
    {
        public int TotalHits { get; set; }

        public List<NuGetQueryPackageModel> Data { get; set; }
    }
}
