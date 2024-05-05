namespace NUServer.Shared.DB
{
    public class PackageModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AuthorName { get; set; }

        public Guid AuthorId { get; set; }

        public UserModel Author { get; set; }

        public string LatestVersion { get; set; }

        public virtual List<PackageVersionModel>? VersionList { get; set; }

        public bool Private { get; set; }

        public long DownloadCount { get; set; }

        public DateTime Published { get; set; }
    }
}
