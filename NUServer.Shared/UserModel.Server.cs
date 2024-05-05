#if SERVER

using Microsoft.AspNetCore.Identity;
using NUServer.Shared.DB;

namespace NUServer.Shared
{
    public partial class UserModel: IdentityUser<Guid>
    {
        public virtual List<PackageModel>? PackageList { get; set; }

        public string? PublishToken { get; set; }
    }
}

#endif