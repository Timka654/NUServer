using NSL.Generators.FillTypeGenerator.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NUServer.Shared.Models.Request
{

    [FillTypeGenerate(typeof(UserModel))]
    public partial class SignUpRequestModel
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

#if CLIENT

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
#endif
    }
}
