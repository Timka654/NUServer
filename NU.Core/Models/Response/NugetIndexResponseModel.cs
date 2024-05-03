using System.Collections.Generic;

namespace NU.Core.Models.Response
{
    public class NuGetIndexResponseModel
    {
        public string Version { get; set; }

        public List<IndexResourceModel> Resources { get; set; }
    }
}
