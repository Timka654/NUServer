namespace NU.Core.Models.Response
{
    public class NuGetQueryPackageVersionModel
    {
        public virtual string Version { get; set; }

        public virtual long Downloads { get; set; }
    }
}
