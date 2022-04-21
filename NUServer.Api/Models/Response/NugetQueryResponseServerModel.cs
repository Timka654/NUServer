using NU.Core.Models.Response;

namespace NUServer.Api.Models.Response
{
    public class NugetQueryResponseServerModel : NugetQueryResponseModel
    {
        public NugetQueryResponseServerModel(NugetQueryPackageServerModel[] data)
        {
            Data = data;
        }
    }
}
