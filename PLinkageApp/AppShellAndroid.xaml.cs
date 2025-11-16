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
            Routing.RegisterRoute("RegisterView1", typeof(RegisterView1));
            Routing.RegisterRoute("RegisterView2", typeof(RegisterView2));
            Routing.RegisterRoute("RegisterView3", typeof(RegisterView3));
            Routing.RegisterRoute("RegisterView4", typeof(RegisterView4));
            Routing.RegisterRoute("RegisterView5", typeof(RegisterView5));
            Routing.RegisterRoute("MessagesView", typeof(MessagesView));
            Routing.RegisterRoute("ViewSkillProviderProfileView", typeof(ViewSkillProviderProfileView));
            Routing.RegisterRoute("ViewProjectOwnerProfileView", typeof(ViewProjectOwnerProfileView));
            Routing.RegisterRoute("ViewProjectView", typeof(ViewProjectView));
            //Routing.RegisterRoute(nameof(AddSkillView), typeof(AddSkillView));
            //Routing.RegisterRoute(nameof(UpdateSkillView), typeof(UpdateSkillView));
            //Routing.RegisterRoute(nameof(ViewSkillView), typeof(ViewSkillView));
            //Routing.RegisterRoute(nameof(AddEducationView), typeof(AddEducationView));
            //Routing.RegisterRoute(nameof(UpdateEducationView), typeof(UpdateEducationView));
            //Routing.RegisterRoute(nameof(ApplyView), typeof(ApplyView));
            //Routing.RegisterRoute(nameof(NegotiatingOfferView), typeof(NegotiatingOfferView));
            //Routing.RegisterRoute(nameof(SendOfferView), typeof(SendOfferView));




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
                new TabbarItem("Logout", "logout.svg", typeof(LogoutView)),
                new TabbarItem("[DEBUG] Send Offer", "home.svg", typeof(SendOfferView)),
                new TabbarItem("[Debug] Update Profile", "home.svg", typeof(UpdateProfileView)),
                new TabbarItem("[Debug] Rate Skill Providers", "home.svg", typeof(RateSkillProviderView)),
                new TabbarItem("[Debug] Resign Skill Providers", "home.svg", typeof(ResignSkillProviderView)),
                new TabbarItem("[Debug] Add Project", "home.svg", typeof(AddProjectView)),
                new TabbarItem("[Debug] Update Project", "home.svg", typeof(UpdateProjectView))
            ];

            return items;
        }
        private TabbarItem[] GetSkillProviderTabbarItems()
        {
            TabbarItem[] items = [
                new TabbarItem("Home", "home.svg", typeof(SkillProviderHomeView)),
                new TabbarItem("Profile", "browsepo.svg", typeof(SkillProviderProfileView)),
                new TabbarItem("Browse", "project.svg", typeof(BrowseProjectView)),
                new TabbarItem("Linkages", "linkages.svg", typeof(SkillProviderLinkagesView)),
                new TabbarItem("Messages", "chat.svg", typeof(ChatView)),
                new TabbarItem("Logout", "logout.svg", typeof(LogoutView)),
                new TabbarItem("[DEBUG] Add Education", "home.svg", typeof(AddEducationView)),
                new TabbarItem("[DEBUG] Add Skill", "home.svg", typeof(AddSkillView)),
                new TabbarItem("[DEBUG] Apply", "home.svg", typeof(ApplyView)),
                new TabbarItem("[DEBUG] Negotiating Offer", "home.svg", typeof(NegotiatingOfferView)),
                new TabbarItem("[DEBUG] Update Education", "home.svg", typeof(UpdateEducationView)),
                new TabbarItem("[DEBUG] Update Skill", "home.svg", typeof(UpdateSkillView)),
                new TabbarItem("[DEBUG] View Skill", "home.svg", typeof(ViewSkillView)),
                new TabbarItem("[Debug] Update Profile", "home.svg", typeof(UpdateProfileView)),
                new TabbarItem("[Debug] Resign Project", "home.svg", typeof(ResignProjectView))
            ];

            return items;
        }




    }
}
