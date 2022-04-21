using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUServer.Models.DB
{
    public class UserModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ShareToken { get; set; }

        public string PublishToken { get; set; }

        [Required]
        public string Email { get; set; }

        public virtual List<PackageModel> PackageList { get; set; }
    }
}
