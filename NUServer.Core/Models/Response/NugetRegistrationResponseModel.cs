using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NU.Core.Models.Response
{
    public class NugetRegistrationResponseModel
    {
        public int Count => Items.Length;

        public NugetRegistrationPageModel[] Items { get; set; }
    }
}
