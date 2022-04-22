using NU.Core;
using NU.Core.Models.Response;
using NUnit.Framework;
using System.Text.Json;

namespace NU.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var nugetPack = new NugetFile(@"D:\Projects\work\my\NSL\NSL\build\Release\package_2022.04.15.1626\NSL.Utils.2022.4.15.1626.nupkg");

            var dir = @"D:\Projects\work\my\NSL\NSL\build\Release\package_2022.04.15.1626\test";

            nugetPack.CreatePackageDirectory(dir);
        }

        [Test]
        public void Test2()
        {
            var obj = JsonSerializer.Deserialize<NugetIndexResponseModel>("{   \"version\": \"3.0.0\",   \"resources\": [     {       \"@id\": \"https://nuget.twicepricegroup.com/api/Package/94708437-6e0c-4a85-a8d2-f808a3947fb0-bf74b521-e71c-4d8e-97fa-e7fc5998255f-f12b95b5-0618-41d4-a4f5-d898d8b2ebc8/v3-flatcontainer\",       \"@type\": \"PackageBaseAddress/3.0.0\",       \"comment\": \"Base URL of where NuGet packages are stored, in the format https://api.nuget.org/v3-flatcontainer/{id-lower}/{version-lower}/{id-lower}.{version-lower}.nupkg\"     },     {       \"@id\": \"https://nuget.twicepricegroup.com/api/Package/94708437-6e0c-4a85-a8d2-f808a3947fb0-bf74b521-e71c-4d8e-97fa-e7fc5998255f-f12b95b5-0618-41d4-a4f5-d898d8b2ebc8/autocomplete\",       \"@type\": \"SearchAutocompleteService/3.0.0-rc\",       \"comment\": \"Autocomplete endpoint of NuGet Search service (primary) used by RC clients\"     },     {       \"@id\": \"https://nuget.twicepricegroup.com/api/Package/94708437-6e0c-4a85-a8d2-f808a3947fb0-bf74b521-e71c-4d8e-97fa-e7fc5998255f-f12b95b5-0618-41d4-a4f5-d898d8b2ebc8/v2/package\",       \"@type\": \"PackagePublish/2.0.0\",       \"comment\": \"\"     },     {       \"@id\": \"https://nuget.twicepricegroup.com/api/Package/94708437-6e0c-4a85-a8d2-f808a3947fb0-bf74b521-e71c-4d8e-97fa-e7fc5998255f-f12b95b5-0618-41d4-a4f5-d898d8b2ebc8/query\",       \"@type\": \"SearchQueryService/Versioned\",       \"comment\": \"Query endpoint of NuGet Search service (primary)\"     },     {       \"@id\": \"https://nuget.twicepricegroup.com/api/Package/94708437-6e0c-4a85-a8d2-f808a3947fb0-bf74b521-e71c-4d8e-97fa-e7fc5998255f-f12b95b5-0618-41d4-a4f5-d898d8b2ebc8/v3/registration3/\",       \"@type\": \"RegistrationsBaseUrl/Versioned\",       \"comment\": \"Base URL of Azure storage where NuGet package registration info is stored used by RC clients. This base URL does not include SemVer 2.0.0 packages.\"     }   ] }", new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}