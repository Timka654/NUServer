using NU.Core.Models;
using NU.Core.Models.Response;

namespace NUServer.Models.Response
{
    public class NuGetIndexResponseServerModel : NuGetIndexResponseModel
    {
        public new IndexResourceModel[] Resources { get; set; }

        public NuGetIndexResponseServerModel(string version, IndexResourceModel[] resources)
        {
            Version = version;
            Resources = resources;
        }
    }
}
