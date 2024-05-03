namespace NU.Core.Models.Response
{
    public class NuGetRegistrationCatalogVulnerabilitiesModel
    {
        //"https://github.com/advisories/ABCD-1234-5678-9012"
        public string AdvisoryUrl { get; set; }

        //"2"
        public string Severity { get; set; }
    }
}
