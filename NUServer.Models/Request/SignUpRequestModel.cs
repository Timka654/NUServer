using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUServer.Models.Request
{
    public class SignUpRequestModel
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
