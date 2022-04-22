using NU.Core.Models.Response;

namespace NUServer.Api.Models.Response
{
    public class NugetAutoCompleteResponseServerModel : NugetAutoCompleteResponseModel
    {
        public NugetAutoCompleteResponseServerModel(int totalHits, List<string> data)
        {
            TotalHits = totalHits;
            Data = data;
        }
    }
}
