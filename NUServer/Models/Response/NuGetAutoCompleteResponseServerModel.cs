using NU.Core.Models.Response;

namespace NUServer.Api.Models.Response
{
    public class NuGetAutoCompleteResponseServerModel : NuGetAutoCompleteResponseModel
    {
        public NuGetAutoCompleteResponseServerModel(int totalHits, List<string> data)
        {
            TotalHits = totalHits;
            Data = data;
        }
    }
}
