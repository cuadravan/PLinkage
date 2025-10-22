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

            //Routing.RegisterRoute("ProjectOwnerUpdateProfileView", typeof(ProjectOwnerUpdateProfileView));
            //Routing.RegisterRoute("ProjectOwnerAddProjectView", typeof(ProjectOwnerAddProjectView));
            //Routing.RegisterRoute("ProjectOwnerUpdateProjectView", typeof(ProjectOwnerUpdateProjectView));
            //Routing.RegisterRoute("ViewProjectView", typeof(ViewProjectView));
            //Routing.RegisterRoute("ProjectOwnerRateSkillProviderView", typeof(ProjectOwnerRateSkillProviderView));
            //Routing.RegisterRoute("ProjectOwnerSendOfferView", typeof(ProjectOwnerSendOfferView));
            //Routing.RegisterRoute("ProjectOwnerSendMessageView", typeof(ProjectOwnerSendMessageView));
            //Routing.RegisterRoute("ViewSkillProviderProfileView", typeof(ViewSkillProviderProfileView));
            //Routing.RegisterRoute("SkillProviderAddEducationView", typeof(SkillProviderAddEducationView));
            //Routing.RegisterRoute("SkillProviderUpdateEducationView", typeof(SkillProviderUpdateEducationView));
            //Routing.RegisterRoute("SkillProviderAddSkillView", typeof(SkillProviderAddSkillView));
            //Routing.RegisterRoute("SkillProviderUpdateSkillView", typeof(SkillProviderUpdateSkillView));
            //Routing.RegisterRoute("SkillProviderSendApplicationView", typeof(SkillProviderSendApplicationView));
            //Routing.RegisterRoute("ViewProjectOwnerProfileView", typeof(ViewProjectOwnerProfileView));
            //Routing.RegisterRoute("RegisterView", typeof(RegisterView));
            Routing.RegisterRoute(nameof(RegisterView1), typeof(RegisterView1));
            Routing.RegisterRoute(nameof(RegisterView2), typeof(RegisterView2));
            Routing.RegisterRoute(nameof(RegisterView3), typeof(RegisterView3));
            Routing.RegisterRoute(nameof(RegisterView4), typeof(RegisterView4));
            Routing.RegisterRoute(nameof(RegisterView5), typeof(RegisterView5));
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
                new TabbarItem("S.Provider", "browsesp.svg", typeof(AdminBrowseSkillProviderView)),
                new TabbarItem("Projects", "project.svg", typeof(AdminBrowseProjectView)),
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
