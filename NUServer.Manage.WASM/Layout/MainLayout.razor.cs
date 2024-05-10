using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using NUServer.Manage.WASM.Services;

namespace NUServer.Manage.WASM.Layout
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject] AppIdentityService identityService { get; set; }

        [Inject] NavigationManager navigationManager { get; set; }

        private async Task Logout()
        {
            await identityService.ClearIdentity();
        }


        Sidebar2 sidebar;
        IEnumerable<NavItem> navItems;

        private async Task<Sidebar2DataProviderResult> SidebarDataProvider(Sidebar2DataProviderRequest request)
        {
            if (navItems is null)
                navItems = GetNavItems();

            return await Task.FromResult(request.ApplyTo(navItems));
        }

        private IEnumerable<NavItem> GetNavItems()
        {
            navItems = new List<NavItem>
        {
            new NavItem { Id = "1", Href = "", IconName = IconName.HouseDoorFill, Text = "Home", Match=NavLinkMatch.All},
            new NavItem { Id = "2", Href = "packages", IconName = IconName.PlusSquareFill, Text = "Packages"},
        };

            return navItems;
        }
    }
}
