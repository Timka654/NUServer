using System.Collections.Generic;

namespace NU.Core.Models.Response
{
    public class NuGetAutoCompleteResponseModel
    {
        public int TotalHits { get; set; }

        public List<string> Data { get; set; }
    }
}
