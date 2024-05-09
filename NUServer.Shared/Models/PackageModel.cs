using NSL.Generators.SelectTypeGenerator.Attributes;

namespace NUServer.Shared.Models
{
    [SelectGenerate("Get", "GetDetails")]
    [SelectGenerateModelJoin("GetDetails", "Get")]
    public partial class PackageModel
    {
        [SelectGenerateInclude("Get")]public Guid Id { get; set; }

        [SelectGenerateInclude("Get")] public string Name { get; set; }

        [SelectGenerateInclude("GetDetails")] public string Description { get; set; }

        public string AuthorName { get; set; }

        public Guid AuthorId { get; set; }

        public UserModel Author { get; set; }

        [SelectGenerateInclude("Get")] public string LatestVersion { get; set; }

        [SelectGenerateInclude("GetDetails")] public virtual List<PackageVersionModel>? VersionList { get; set; }

        [SelectGenerateInclude("Get")] public bool Private { get; set; }

        [SelectGenerateInclude("Get")] public long DownloadCount { get; set; }

        [SelectGenerateInclude("Get")] public DateTime Published { get; set; }
    }
}
