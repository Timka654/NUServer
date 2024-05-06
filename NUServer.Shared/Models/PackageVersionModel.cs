using NSL.Generators.SelectTypeGenerator.Attributes;

namespace NUServer.Shared.Models
{
    public class PackageVersionModel
    {
        public Guid PackageId { get; set; }

        public virtual PackageModel? Package { get; set; }

        [SelectGenerateInclude("GetDetails")] public string Version { get; set; }

        [SelectGenerateInclude("GetDetails")] public DateTime UploadTime { get; set; }

        [SelectGenerateInclude("GetDetails")] public long DownloadCount { get; set; }

        public virtual List<PackageVersionDependencyGroupModel>? DependencyGroupList { get; set; }
    }
}
