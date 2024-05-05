namespace NUServer.Shared.DB
{
    public class PackageVersionDependencyModel
    {
        public Guid Id { get; set; }

        public Guid GroupId { get; set; }

        public virtual PackageVersionDependencyGroupModel? Group { get; set; }

        public string DependencyName { get; set; }

        public string DependencyVersion { get; set; }

        public bool External { get; set; }
    }
}
