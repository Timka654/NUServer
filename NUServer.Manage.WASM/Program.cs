using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NSL.ASPNET.Identity.ClientIdentity.Providers;
using NSL.ASPNET.Identity.ClientIdentity;
using NUServer.Manage.WASM;
using NUServer.Manage.WASM.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var baseUri = new Uri(builder.HostEnvironment.BaseAddress);



//Console.WriteLine($" api uri = '{baseUri.GetLeftPart(System.UriPartial.Authority)}'");

builder.Services.AddHttpClient("Default", (sp, client) => client.FillHttpClientIdentity(sp).BaseAddress = new Uri(baseUri.GetLeftPart(System.UriPartial.Authority)))
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
{
    AllowAutoRedirect = false
})
.AddHttpMessageIdentityHandler();

builder.Services.AddBlazorBootstrap();
builder.Services.AddBlazoredLocalStorageAsSingleton();

builder.Services
    .AddIdentityPolicyProvider<IdentityPolicyProvider>()
    .AddHttpMessageIdentityHandler()
    .AddIdentityStateProvider<IdentityStateProvider>()
    .AddIdentityService<AppIdentityService>()
    .AddIdentityAuthorizationService<IdentityAuthorizationService>(); 


builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");



await builder.Build().RunAsync();
