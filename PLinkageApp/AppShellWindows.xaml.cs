using Microsoft.Maui.Controls;
using PLinkageApp.Views;
using PLinkageApp.ViewsWindows;
using PLinkageApp.ViewModels;
using PLinkageApp.Interfaces;
using PLinkageShared.Enums;

namespace PLinkageApp
{
    public partial class AppShellWindows : Shell
    {
        private readonly IStartupService _startupService;
        private bool _initialized;

        public AppShellWindows(AppShellViewModel viewModel, IStartupService startupService)
        {
            InitializeComponent();
            RegisterRoutes(); // Moved routes to helper method for cleanliness

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

        record FlyoutMenuItem(string Name, string Icon, Type Type);

        public void ClearFlyout()
        {
            var itemsToRemove = Items
                                .Where(item => item.Route != "AuthWrapper")
                                .ToList();

            foreach (var item in itemsToRemove)
            {
                Items.Remove(item);
            }
        }

        public void ConfigureFlyout(UserRole? role)
        {
            if (role == null) return;

            // B. Get new items 
            FlyoutMenuItem[] menuItems = role switch
            {
                UserRole.Admin => GetAdminMenuItems(),
                UserRole.SkillProvider => GetSkillProviderMenuItems(),
                UserRole.ProjectOwner => GetProjectOwnerMenuItems(),
                _ => []
            };

            // C. Add them to the Shell
            foreach (var item in menuItems)
            {
                // In Flyout, we wrap the Page in a ShellContent, and wrap that in a FlyoutItem
                var flyoutItem = new FlyoutItem
                {
                    Title = item.Name,
                    Icon = item.Icon, // Ensure your icon string is valid for Windows or use explicit ImageSource
                    Route = item.Type.Name // Use class name as Route
                };

                // Add the content to the flyout item
                flyoutItem.Items.Add(new ShellContent
                {
                    ContentTemplate = new DataTemplate(item.Type),
                    Route = item.Type.Name
                });

                // Add to Shell
                Items.Add(flyoutItem);
            }
        }

        // --- MENU DEFINITIONS ---

        private FlyoutMenuItem[] GetAdminMenuItems() =>
        [
            new("Home", "homewin.png", typeof(AdminHomeView)),
            new("Browse Projects", "projectwin.png", typeof(BrowseProjectView)),
            new("Browse Project Owners", "browsepowin.png", typeof(AdminBrowseProjectOwnerView)),
            new("Browse Skill Providers", "browsespwin.png", typeof(BrowseSkillProviderView)),
            new("My Messages", "chatwin.png", typeof(ChatWindowsView))
        ];

        private FlyoutMenuItem[] GetProjectOwnerMenuItems() =>
        [
            new("Home", "homewin.png", typeof(ProjectOwnerHomeView)),
            new("My Profile", "browsepowin.png", typeof(ProjectOwnerProfileView)),
            new("Browse Skill Providers", "browsespwin.png", typeof(BrowseSkillProviderView)),
            new("Your Linkages", "linkageswin.png", typeof(ProjectOwnerLinkagesView)),
            new("My Messages", "chatwin.png", typeof(ChatWindowsView))
        ];

        private FlyoutMenuItem[] GetSkillProviderMenuItems() =>
        [
            new("Home", "homewin.png", typeof(SkillProviderHomeView)),
            new("My Profile", "browsepowin.png", typeof(SkillProviderProfileView)),
            new("Browse Projects", "projectwin.png", typeof(BrowseProjectView)),
            new("Your Linkages", "linkageswin.png", typeof(SkillProviderLinkagesView)),
            new("My Messages", "chatwin.png", typeof(ChatWindowsView))
        ];

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(UpdateProfileView), typeof(UpdateProfileView));
            Routing.RegisterRoute(nameof(AddProjectView), typeof(AddProjectView));
            Routing.RegisterRoute(nameof(UpdateProjectView), typeof(UpdateProjectView));
            Routing.RegisterRoute(nameof(ViewProjectView), typeof(ViewProjectView));
            Routing.RegisterRoute(nameof(RateSkillProviderView), typeof(RateSkillProviderView));
            Routing.RegisterRoute(nameof(SendOfferView), typeof(SendOfferView));
            Routing.RegisterRoute(nameof(ViewSkillProviderProfileView), typeof(ViewSkillProviderProfileView));
            Routing.RegisterRoute(nameof(AddEducationView), typeof(AddEducationView));
            Routing.RegisterRoute(nameof(UpdateEducationView), typeof(UpdateEducationView));
            Routing.RegisterRoute(nameof(AddSkillView), typeof(AddSkillView));
            Routing.RegisterRoute(nameof(ApplyView), typeof(ApplyView));
            Routing.RegisterRoute(nameof(ViewProjectOwnerProfileView), typeof(ViewProjectOwnerProfileView));
            Routing.RegisterRoute(nameof(RegisterView1), typeof(RegisterView1));
            Routing.RegisterRoute(nameof(RegisterView2), typeof(RegisterView2));
            Routing.RegisterRoute(nameof(RegisterView3), typeof(RegisterView3));
            Routing.RegisterRoute(nameof(RegisterView4), typeof(RegisterView4));
            Routing.RegisterRoute(nameof(RegisterView5), typeof(RegisterView5));
            Routing.RegisterRoute(nameof(ViewSkillView), typeof(ViewSkillView));
            Routing.RegisterRoute(nameof(MessagesView), typeof(MessagesView));
            Routing.RegisterRoute(nameof(ResignProjectView), typeof(ResignProjectView));
            Routing.RegisterRoute(nameof(NegotiatingOfferView), typeof(NegotiatingOfferView));
            Routing.RegisterRoute(nameof(ProcessResignationView), typeof(ProcessResignationView));
        }
    }
}