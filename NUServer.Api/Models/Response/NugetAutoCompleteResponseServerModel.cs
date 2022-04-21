using NU.Core.Models.Response;

namespace NUServer.Api.Models.Response
{
    public class NugetAutoCompleteResponseServerModel : NugetAutoCompleteResponseModel
    {
        public NugetAutoCompleteResponseServerModel(string[] data)
        {
            Data = data;
        }
    }
}
