﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NU.Core.Models.Response
{
    public class NugetRegistrationCatalogEntryModel
    {
        //"https://api.nuget.org/v3/catalog0/data/2017.10.05.18.41.33/nuget.server.core.3.0.0-beta.json"
        [JsonPropertyName("@id")]
        public virtual string Url { get; set; }

        //".NET Foundation",
        public string Authors { get; set; }

        public virtual NugetRegistrationCatalogDepedencyGroupModel[] DependencyGroups { get; set; }

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
        public virtual string PackageContentUrl { get; set; }

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
}