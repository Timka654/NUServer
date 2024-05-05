using Blazored.LocalStorage;
using NSL.ASPNET.Identity.ClientIdentity;
using NSL.ASPNET.Identity.ClientIdentity.Providers;
using NSL.ASPNET.Identity.JWT;
using NSL.Generators.HttpEndPointGenerator.Shared.Attributes;
using NUServer.Shared.Models.Controllers;
using System.Net.Http;

namespace NUServer.Manage.WASM.Services
{
    [HttpEndPointImplementGenerate(typeof(IManageIdentityController))]
    public partial class AppIdentityService(IdentityStateProvider identityStateProvider, IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService) : IdentityJWTService(identityStateProvider)
    {
        private const string tokenStoreName = "authToken";

        protected partial HttpClient CreateEndPointClient(string url)
            => httpClientFactory.CreateClient("Default");

        protected override async Task<string?> ReadToken()
        {
            if (await localStorageService.ContainKeyAsync(tokenStoreName))
                return await localStorageService.GetItemAsStringAsync(tokenStoreName);

            return default;
        }

        protected override async Task SaveToken(string? token)
        {
            if (token == default)
                await localStorageService.RemoveItemAsync(tokenStoreName);
            else
                await localStorageService.SetItemAsStringAsync(tokenStoreName, token);
        }
    }
}
