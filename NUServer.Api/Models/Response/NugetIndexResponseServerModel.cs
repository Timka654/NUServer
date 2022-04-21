using NU.Core.Models;
using NU.Core.Models.Response;
using NUServer.Models;

namespace NUServer.Api.Models.Response
{
    public class NugetIndexResponseServerModel : NugetIndexResponseModel
    {
        public new IndexResourceModel[] Resources { get; set; }

        public NugetIndexResponseServerModel(string version, IndexResourceModel[] resources)
        {
            Version = version;
            Resources = resources;
        }
    }
}
