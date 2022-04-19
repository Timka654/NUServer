using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUServer.Models.Response
{
    public class NugetAutoCompleteResponseModel
    {
        public int TotalHits => Data.Length;

        public string[] Data { get; set; }
    }
}
