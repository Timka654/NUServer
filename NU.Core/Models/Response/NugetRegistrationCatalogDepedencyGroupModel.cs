﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NU.Core.Models.Response
{
    public class NugetRegistrationCatalogDepedencyGroupModel
    {
        //"https://api.nuget.org/v3/catalog0/data/2017.10.05.18.41.33/nuget.server.core.3.0.0-beta.json#dependencygroup"
        [JsonPropertyName("@id")]
        public virtual string Url { get; set; }

        public virtual string TargetFramework { get; set; }

        public virtual NugetRegistrationCatalogDepedencyModel[] Dependencies { get; set; }
    }
}