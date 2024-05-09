using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NU.SimpleClient.Models.Response
{
    public class SignUpResponseModel
    {
        public Guid UID { get; set; }
        
        public string ShareToken { get; set; }
        
        public string PublishToken { get; set; }
    }
}
