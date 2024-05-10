using Microsoft.AspNetCore.Components;
using NUServer.Manage.WASM.Services;
using NUServer.Shared.Models;

namespace NUServer.Manage.WASM.Pages
{
    public partial class PackageDetailsPage : ComponentBase
    {
        [Inject] PackagesService PackagesService { get; set; }

        [Inject] NavigationManager NavigationManager { get; set; }

        [Parameter] public Guid Id { get; set; }

        private PackageModel? Details { get; set; }

        protected override async void OnInitialized()
        {
            var response = await PackagesService.ManagePackagesDetailsPostRequest(Id);

            if (!response.IsSuccess)
                return;

            Details = response.Data;

            StateHasChanged();
        }

        async Task RemoveClickHandle()
        {
            var response = await PackagesService.ManagePackagesRemovePostRequest(Id);

            if (!response.IsSuccess)
                return;

            NavigationManager.NavigateTo("packages");

        }

        async Task RemoveVersionClickHandle(PackageVersionModel version)
        {
            var response = await PackagesService.ManagePackagesRemoveVersionPostRequest(new Shared.Models.Request.RemovePackageVersionRequestModel() { PackageId = Id, PackageVersion = version.Version });

            if (!response.IsSuccess)
                return;

            if (response.Data)
            {
                NavigationManager.NavigateTo("packages");
                return;
            }

            Details.VersionList.Remove(version);

            StateHasChanged();
        }
    }
}
