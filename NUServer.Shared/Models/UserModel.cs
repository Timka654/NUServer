using NUServer.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace NUServer.Shared.Models
{
    public partial class UserModel
    {
        public string Name { get; set; }

        public string? ShareToken { get; set; }

        public string? PublishToken { get; set; }
    }
}
