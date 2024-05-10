using Microsoft.AspNetCore.Components;
using NUServer.Manage.WASM.Services;
using NUServer.Shared.Models;

namespace NUServer.Manage.WASM.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject] AppIdentityService AppIdentityService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        private UserModel? Details { get; set; }

        private bool showPublishToken { get; set; }
        private bool showSharedToken { get; set; }

        private string BaseUrl => new Uri(NavigationManager.BaseUri).GetLeftPart(System.UriPartial.Authority);

        protected override async Task OnInitializedAsync()
        {
            var response = await AppIdentityService.ManageUserGetStorageDataPostRequest();

            if (!response.IsSuccess)
                return;

            Details = response.Data;
        }

        async Task RefreshSharedClickHandle()
        {
            var response = await AppIdentityService.ManageUserRefreshStorageShareTokenPostRequest();

            if (!response.IsSuccess)
                return;

            Details.ShareToken = response.Data;


            StateHasChanged();
        }

        async Task RefreshPublishClickHandle()
        {
            var response = await AppIdentityService.ManageUserRefreshStoragePublishTokenPostRequest();

            if (!response.IsSuccess)
                return;

            Details.PublishToken = response.Data;


            StateHasChanged();
        }
    }
}
