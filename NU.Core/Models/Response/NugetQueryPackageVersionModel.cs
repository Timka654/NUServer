namespace NU.Core.Models.Response
{
    public class NugetQueryPackageVersionModel
    {
        public virtual string Version { get; set; }

        public virtual long Downloads { get; set; }
    }
}
