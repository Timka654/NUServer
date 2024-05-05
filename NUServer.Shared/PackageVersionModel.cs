namespace NUServer.Shared.DB
{
    public class PackageVersionModel
    {
        public Guid PackageId { get; set; }

        public virtual PackageModel? Package { get; set; }

        public string Version { get; set; }

        public DateTime UploadTime { get; set; }

        public long DownloadCount { get; set; }

        public virtual List<PackageVersionDependencyGroupModel>? DepedencyGroupList { get; set; }
    }
}
