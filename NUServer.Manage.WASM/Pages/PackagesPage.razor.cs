using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NSL.Database.EntityFramework.Filter;
using NSL.Database.EntityFramework.Filter.Models;
using NSL.HttpClient;
using NSL.HttpClient.Blazor.Validators;
using NUServer.Manage.WASM.Services;
using NUServer.Shared.Models;

namespace NUServer.Manage.WASM.Pages
{
    public partial class PackagesPage : ComponentBase
    {
        [Inject] PackagesService PackagesService { get; set; }

        private FilterResultModel<PackageModel> viewData = new FilterResultModel<PackageModel>()
        {
            Data = []
        };

        private async Task LoadPackages()
        {
            var response = await PackagesService.ManagePackagesGetPostRequest(NavigationFilterBuilder.Create().ToFilter());

            if (!response.IsSuccess)
                return;

            viewData = response.Data;

            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadPackages();
        }
        ServerModelValidator validator { get; set; }

        async Task UploadFileHandle(InputFileChangeEventArgs args)
        {
            var response = await PackagesService.ManagePackagesUploadPackagePostRequest(new NSL.HttpClient.HttpContent.StreamDataContent() { FileName = args.File.Name, Stream = args.File.OpenReadStream() }, BaseHttpRequestOptions.Create(validator));

            if (!response.IsSuccess)
                return;
            IEnumerable<PackageModel> q = viewData.Data;


            if (viewData.Data.Any(x => x.Id == response.Data.Id))
            {
                q = viewData.Data.Where(x => x.Id != response.Data.Id).ToArray();
            }

            viewData.Data = q.Prepend(response.Data).ToArray();

            StateHasChanged();
        }
    }
}
