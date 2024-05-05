using NUServer.Shared.DB;
using System.ComponentModel.DataAnnotations;

namespace NUServer.Shared
{
    public partial class UserModel
    {
        public string? ShareToken { get; set; }
    }
}
