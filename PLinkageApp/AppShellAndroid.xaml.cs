using Microsoft.Maui.Controls;
using PLinkageApp.Views;
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

            Routing.RegisterRoute(nameof(RegisterView1), typeof(RegisterView1));
            Routing.RegisterRoute(nameof(RegisterView2), typeof(RegisterView2));
            Routing.RegisterRoute(nameof(RegisterView3), typeof(RegisterView3));
            Routing.RegisterRoute(nameof(RegisterView4), typeof(RegisterView4));
            Routing.RegisterRoute(nameof(RegisterView5), typeof(RegisterView5));
            Routing.RegisterRoute(nameof(MessagesView), typeof(MessagesView));
            Routing.RegisterRoute(nameof(ViewSkillProviderProfileView), typeof(ViewSkillProviderProfileView));
            Routing.RegisterRoute(nameof(ViewProjectOwnerProfileView), typeof(ViewProjectOwnerProfileView));
            Routing.RegisterRoute(nameof(ViewProjectView), typeof(ViewProjectView));
            Routing.RegisterRoute(nameof(AddSkillView), typeof(AddSkillView));
            Routing.RegisterRoute(nameof(ViewSkillView), typeof(ViewSkillView));
            Routing.RegisterRoute(nameof(AddEducationView), typeof(AddEducationView));
            Routing.RegisterRoute(nameof(UpdateEducationView), typeof(UpdateEducationView));
            Routing.RegisterRoute(nameof(UpdateProfileView), typeof(UpdateProfileView));
            Routing.RegisterRoute(nameof(ApplyView), typeof(ApplyView));
            Routing.RegisterRoute(nameof(ResignProjectView), typeof(ResignProjectView));
            Routing.RegisterRoute(nameof(NegotiatingOfferView), typeof(NegotiatingOfferView));
            Routing.RegisterRoute(nameof(ProcessResignationView), typeof(ProcessResignationView));
            Routing.RegisterRoute(nameof(SendOfferView), typeof(SendOfferView));
            Routing.RegisterRoute(nameof(AddProjectView), typeof(AddProjectView));
            Routing.RegisterRoute(nameof(UpdateProjectView), typeof(UpdateProjectView));
            Routing.RegisterRoute(nameof(RateSkillProviderView), typeof(RateSkillProviderView));

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
                new TabbarItem("S.Provider", "browsesp.svg", typeof(BrowseSkillProviderView)),
                new TabbarItem("Projects", "project.svg", typeof(BrowseProjectView)),
                new TabbarItem("P.Owner", "browsepo.svg", typeof(AdminBrowseProjectOwnerView)),
                new TabbarItem("Messages", "chat.svg", typeof(ChatView)),
                new TabbarItem("Logout", "logout.svg", typeof(LogoutView))
            ];

            return items;

        }
        private TabbarItem[] GetProjectOwnerTabBarItems()
        {
            TabbarItem[] items = [
                new TabbarItem("Home", "home.svg", typeof(ProjectOwnerHomeView)),
                new TabbarItem("Profile", "browsepo.svg", typeof(ProjectOwnerProfileView)),
                new TabbarItem("Browse", "browsesp.svg", typeof(BrowseSkillProviderView)),
                new TabbarItem("Linkages", "linkages.svg", typeof(ProjectOwnerLinkagesView)),
                new TabbarItem("Messages", "chat.svg", typeof(ChatView)),
                new TabbarItem("Logout", "logout.svg", typeof(LogoutView))
            ];

            return items;
        }
        private TabbarItem[] GetSkillProviderTabbarItems()
        {
            TabbarItem[] items = [
                new TabbarItem("Browse", "project.svg", typeof(BrowseProjectView)),
                new TabbarItem("Messages", "chat.svg", typeof(ChatView)),
                new TabbarItem("Logout", "logout.svg", typeof(LogoutView))
            ];

            return items;
        }




    }
}
