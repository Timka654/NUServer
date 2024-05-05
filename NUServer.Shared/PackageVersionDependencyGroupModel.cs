namespace NUServer.Shared.DB
{
    public class PackageVersionDependencyGroupModel
    {
        public Guid Id { get; set; }

        public Guid PackageId { get; set; }

        public virtual PackageModel? Package { get; set; }

        public string Version { get; set; }

        public virtual PackageVersionModel? PackageVersion { get; set; }

        public string TargetFramework { get; set; }

        public List<PackageVersionDependencyModel>? Dependencies { get; set; }
    }
}
