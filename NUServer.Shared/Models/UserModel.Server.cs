#if SERVER

using Microsoft.AspNetCore.Identity;
using NUServer.Shared.Models;

namespace NUServer.Shared.Models
{
    public partial class UserModel: IdentityUser<Guid>
    {
        public virtual List<PackageModel>? PackageList { get; set; }
    }
}

#endif