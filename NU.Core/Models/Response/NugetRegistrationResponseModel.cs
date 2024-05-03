using System.Collections.Generic;

namespace NU.Core.Models.Response
{
    public class NuGetRegistrationResponseModel
    {
        public int Count => Items.Count;

        public List<NuGetRegistrationPageModel> Items { get; set; }
    }
}
