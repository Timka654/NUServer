using System.ComponentModel.DataAnnotations;

namespace NUServer.Shared.DB
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
