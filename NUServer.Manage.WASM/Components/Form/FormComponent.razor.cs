using Microsoft.AspNetCore.Components;
using NSL.HttpClient;
using NSL.HttpClient.Blazor.Validators;

namespace NUServer.Manage.WASM.Components.Form
{
    public partial class FormComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

        [Parameter] public object Model { get; set; }

        public ServerModelValidator ApiValidator { get; private set; }

        [Parameter] public RenderFragment<FormComponent> ChildContent { get; set; }

        [Parameter] public EventCallback<FormComponent> OnSubmit { get; set; }

        public BaseHttpRequestOptions CreateOptions()
            => BaseHttpRequestOptions.Create(ApiValidator);
    }
}
