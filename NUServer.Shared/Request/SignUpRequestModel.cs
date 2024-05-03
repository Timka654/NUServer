using System.ComponentModel.DataAnnotations;

namespace NUServer.Shared.Request
{
    public class SignUpRequestModel
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
