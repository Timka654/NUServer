using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NUServer.Models.Response
{
    public class NugetRegistrationResponseModel
    {
        public int Count => Items.Length;

        public NugetRegistrationPageModel[] Items { get; set; }

        public NugetRegistrationResponseModel(PackageModel package, Func<string, string?, string> registrationUrl, Func<string, string?, string> nupkgUrl)
        {
            Items = new NugetRegistrationPageModel[] { new NugetRegistrationPageModel(package, registrationUrl, nupkgUrl) };
        }
    }

    public class NugetRegistrationPageModel
    {
        public NugetRegistrationPageModel(PackageModel package, Func<string, string?, string> registrationUrl, Func<string, string?, string> nupkgUrl)
        {
            Items = package.VersionList.Select(x => new NugetRegistrationLeafModel(package, x, registrationUrl, nupkgUrl)).ToArray();

            Lower = package.VersionList.OrderBy(x => x.UploadTime).First().Version;

            Upper = package.LatestVersion;

            Url = registrationUrl(package.Name, null) + $"#page/{Upper}/{Upper}";
        }

        //"https://api.nuget.org/v3/registration3/nuget.server.core/index.json#page/3.0.0-beta/3.0.0-beta",
        [JsonPropertyName("@id")]
        public string Url { get; set; }

        public int Count => Items.Length;

        public NugetRegistrationLeafModel[] Items { get; set; }

        //"3.0.0-beta",
        public string Lower { get; set; }

        //"3.0.0-beta"
        public string Upper { get; set; }

    }

    public class NugetRegistrationLeafModel
    {
        public NugetRegistrationLeafModel(PackageModel package, PackageVersionModel version, Func<string, string?, string> registrationUrl, Func<string, string?, string> nupkgUrl)
        {
            Url = registrationUrl(package.Name, version.Version);
            Registration = registrationUrl(package.Name, null);

            PackageContent = nupkgUrl(package.Name, version.Version);

            CatalogEntry = new NugetRegistrationCatalogEntryModel(package, version, registrationUrl, nupkgUrl);

        }

        //https://api.nuget.org/v3/registration3/nuget.server.core/3.0.0-beta.json
        [JsonPropertyName("@id")]
        public string Url { get; set; }

        public NugetRegistrationCatalogEntryModel CatalogEntry { get; set; }

        //"https://api.nuget.org/v3-flatcontainer/nuget.server.core/3.0.0-beta/nuget.server.core.3.0.0-beta.nupkg",
        public string PackageContent { get; set; }

        //"https://api.nuget.org/v3/registration3/nuget.server.core/index.json"
        public string Registration { get; set; }
    }


    public class NugetRegistrationCatalogEntryModel
    {
        public NugetRegistrationCatalogEntryModel(PackageModel package, PackageVersionModel version, Func<string, string?, string> registrationUrl, Func<string, string?, string> nupkgUrl)
        {
            Authors = package.Avtor.Name;
            Description = package.Description;
            Id = package.Name;

            PackageContentUrl = nupkgUrl(package.Name, version.Version);
            Version = version.Version;
            Published = version.UploadTime;

            DependencyGroups = version.DepedencyGroupList.Select(x => new NugetRegistrationCatalogDepedencyGroupModel(package, version, x, registrationUrl)).ToArray();
        }

        //"https://api.nuget.org/v3/catalog0/data/2017.10.05.18.41.33/nuget.server.core.3.0.0-beta.json"
        [JsonPropertyName("@id")]
        public string Url { get; set; }

        //".NET Foundation",
        public string Authors { get; set; }

        public NugetRegistrationCatalogDepedencyGroupModel[] DependencyGroups { get; set; }

        //"Core library for creating a Web Application used to host a simple NuGet feed"
        public string Description { get; set; }

        //"",
        public string IconUrl { get; set; }

        //"NuGet.Server.Core",
        public string Id { get; set; }

        //"",
        public string Launguage { get; set; }

        //"https://raw.githubusercontent.com/NuGet/NuGet.Server/dev/LICENSE.txt",
        public string LicenseUrl { get; set; }

        //true
        public bool Listed { get; set; } = true;

        //"2.6",
        public string MinClientVersion { get; set; }

        //"https://api.nuget.org/v3-flatcontainer/nuget.server.core/3.0.0-beta/nuget.server.core.3.0.0-beta.nupkg",
        [JsonPropertyName("packageContent")]
        public string PackageContentUrl { get; set; }

        //"https://github.com/NuGet/NuGet.Server",
        public string ProjectUrl { get; set; }

        //"2017-10-05T18:40:32.43+00:00",
        public DateTime Published { get; set; }

        //false,
        public bool RequireLicenseAcceptance { get; set; }

        //"",
        public string Summary { get; set; }

        //[ "" ],
        public string[] Tags { get; set; } = new string[] { "" };

        //"",
        public string Title { get; set; }

        //"3.0.0-beta"
        public string Version { get; set; }

        public NugetRegistrationCatalogVulnerabilitiesModel[] Vulnerabilities { get; set; }

    }

    public class NugetRegistrationCatalogDepedencyGroupModel
    {
        public NugetRegistrationCatalogDepedencyGroupModel(PackageModel package, PackageVersionModel version, PackageVersionDepedencyGroupModel group, Func<string, string?, string> registrationUrl)
        {
            TargetFramework = group.TargetFramework;
            Dependencies = group.Depedencies.Select(x => new NugetRegistrationCatalogDepedencyModel(x, package,version, registrationUrl)).ToArray();
        }

        //"https://api.nuget.org/v3/catalog0/data/2017.10.05.18.41.33/nuget.server.core.3.0.0-beta.json#dependencygroup"
        [JsonPropertyName("@id")]
        public string Url { get; set; }

        public string TargetFramework { get; set; }

        public NugetRegistrationCatalogDepedencyModel[] Dependencies { get; set; }
    }

    public class NugetRegistrationCatalogDepedencyModel
    {
        public NugetRegistrationCatalogDepedencyModel(PackageVersionDepedencyModel x, PackageModel package, PackageVersionModel version, Func<string, string?, string> registrationUrl)
        {
            Registration = registrationUrl(package.Name, version.Version);
            Name = x.DepedencyName;
            Range = $"[{x.DepedencyVersion}, )";
        }

        //"https://api.nuget.org/v3/catalog0/data/2017.10.05.18.41.33/nuget.server.core.3.0.0-beta.json#dependencygroup/nuget.core"
        [JsonPropertyName("@id")]
        public string Url { get; set; }

        //"NuGet.Core",
        [JsonPropertyName("id")]
        public string Name { get; set; }

        //"[2.14.0, )"
        public string Range { get; set; }

        //"https://api.nuget.org/v3/registration3/nuget.core/index.json"
        public string Registration { get; set; }
    }

    public class NugetRegistrationCatalogVulnerabilitiesModel
    {
        //"https://github.com/advisories/ABCD-1234-5678-9012"
        public string AdvisoryUrl { get; set; }

        //"2"
        public string Severity { get; set; }
    }
}
