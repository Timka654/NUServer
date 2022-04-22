using System.Collections.Generic;

namespace NU.Core.Models.Response
{
    public class NugetRegistrationResponseModel
    {
        public int Count => Items.Count;

        public List<NugetRegistrationPageModel> Items { get; set; }
    }
}
