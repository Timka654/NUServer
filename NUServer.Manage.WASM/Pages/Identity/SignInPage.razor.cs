using Microsoft.AspNetCore.Components;
using NUServer.Manage.WASM.Components.Form;
using NUServer.Manage.WASM.Services;
using NUServer.Shared.Models.Request;

namespace NUServer.Manage.WASM.Pages.Identity
{
    public partial class SignInPage : ComponentBase
    {
        [Inject] AppIdentityService identityService {  get; set; }

        [Inject] NavigationManager navigationManager { get; set; }
        
        private SignInRequestModel requestData { get; set; } = new SignInRequestModel();

        protected override Task OnInitializedAsync()
        {
            if (identityService.IsAuthenticated)
                navigationManager.NavigateTo("");

            return base.OnInitializedAsync();
        }

        private async Task onSubmit(FormComponent form)
        {
            var response = await identityService.ManageIdentitySignInPostRequest(requestData, form.CreateOptions());

            if (response.IsSuccess)
            {
                await identityService.SetIdentity(response.Data.Token);

                navigationManager.NavigateTo("");
            }
        }
    }
}
