using NU.Core.Models.Response;

namespace NUServer.Models.Response
{
    public class NuGetQueryResponseServerModel : NuGetQueryResponseModel
    {
        public NuGetQueryResponseServerModel(int totalHits, List<NuGetQueryPackageServerModel> data)
        {
            TotalHits = totalHits;
            Data = new List<NuGetQueryPackageModel>(data);
        }
    }
}
