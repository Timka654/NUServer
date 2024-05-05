#if CLIENT

namespace NUServer.Shared
{
    public partial class UserModel
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
    }
}

#endif