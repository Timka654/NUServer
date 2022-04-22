using NU.Core.Models.Response;

namespace NUServer.Api.Models.Response
{
    public class NugetQueryResponseServerModel : NugetQueryResponseModel
    {
        public NugetQueryResponseServerModel(int totalHits, List<NugetQueryPackageServerModel> data)
        {
            TotalHits = totalHits;
            Data = new List<NugetQueryPackageModel>(data);
        }
    }
}
