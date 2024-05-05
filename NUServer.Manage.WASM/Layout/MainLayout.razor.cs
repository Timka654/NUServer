using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace NUServer.Manage.WASM.Layout
{
    public partial class MainLayout : LayoutComponentBase
    {
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
            new NavItem { Id = "1", Href = "/", IconName = IconName.HouseDoorFill, Text = "Home", Match=NavLinkMatch.All},
            new NavItem { Id = "2", Href = "/counter", IconName = IconName.PlusSquareFill, Text = "Counter"},
            new NavItem { Id = "3", Href = "/weather", IconName = IconName.Table, Text = "Fetch Data"},
        };

            return navItems;
        }
    }
}
