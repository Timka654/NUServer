using NUServer.Shared.Models;

namespace NUServer.Models
{
    public class PublishPackageResultModel
    {
        public string? ErrorField { get; set; }

        public string? ErrorMessage { get; set; }

        public PackageModel? Package { get; set; }
    }
}
