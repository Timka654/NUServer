using NU.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUServer.Models.DB
{
    public class ResourceModel : IndexResourceModel
    {
        public int Id { get; set; }

        public bool Active { get; set; }
    }
}
