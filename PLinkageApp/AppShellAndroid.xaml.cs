using Microsoft.Maui.Controls;
using PLinkageApp.ViewsAndroid;
using PLinkageApp.ViewModels;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;

namespace PLinkageApp
{
    public partial class AppShellAndroid : Shell
    {
        private readonly IStartupService _startupService;
        private bool _initialized;

        public AppShellAndroid(AppShellViewModel viewModel, IStartupService startupService)
        {
            InitializeComponent();
            _startupService = startupService;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_initialized)
                return;

            _initialized = true;
            await _startupService.StartAsync();
        }
        public void ConfigureTabs(UserRole? role)
        {
            if (role == null)
                return;

            TabbarItem[] items = [];

            if (role == UserRole.Admin)
            {
                items = GetAdminTabbarItems();
            }
            else if(role == UserRole.SkillProvider)
            {
                items = GetSkillProviderTabbarItems();
            }
            else
            {
                items = GetProjectOwnerTabBarItems();
            }

                var tabbarItems = items.Select(i => new ShellContent
                {
                    Title = i.Name,
                    Icon = i.Icon,
                    Route = i.Type.Name, // Use the class name as route
                    ContentTemplate = new DataTemplate(i.Type)
                });
            MainTabBar.Items.Clear();
            foreach (var item in tabbarItems)
            {
                MainTabBar.Items.Add(item);
            }
        }

        record TabbarItem(string Name, string Icon, Type Type);

        private TabbarItem[] GetAdminTabbarItems()
        {
            TabbarItem[] items = [
                new TabbarItem("Home", "home.svg", typeof(AdminHomeView)),
                new TabbarItem("Logout", "logout.svg", typeof(LogoutView))
            ];

            return items;

        }
        private TabbarItem[] GetProjectOwnerTabBarItems()
        {
            TabbarItem[] items = [
                new TabbarItem("Home", "home.svg", typeof(ProjectOwnerHomeView)),
                new TabbarItem("Logout", "logout.svg", typeof(LogoutView))
            ];

            return items;
        }
        private TabbarItem[] GetSkillProviderTabbarItems()
        {
            TabbarItem[] items = [
                new TabbarItem("Home", "home.svg", typeof(SkillProviderHomeView)),
                new TabbarItem("Logout", "logout.svg", typeof(LogoutView))
            ];

            return items;
        }




    }
}
