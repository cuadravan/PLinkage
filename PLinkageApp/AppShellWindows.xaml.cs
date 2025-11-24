using Microsoft.Maui.Controls;
using PLinkageApp.Views;
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

        // --- NEW LOGIC STARTS HERE ---

        // 1. Define the Record (Same as your Android version)
        record FlyoutMenuItem(string Name, string Icon, Type Type);

        public void ClearFlyout()
        {
            // A. Remove existing dynamic items
            // We iterate backwards or use ToList() to safely modify the collection while looping.
            // We keep "StartView" and "LoginView" because they are defined in XAML.

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
            new("Home", "home.png", typeof(AdminHomeView)),
            new("Browse Projects", "project.png", typeof(BrowseProjectsView)),
            new("Browse Project Owners", "browsepo.png", typeof(AdminBrowseProjectOwnerView)),
            new("Browse Skill Providers", "browsesp.png", typeof(BrowseSkillProvidersView)),
            new("My Messages", "chat.png", typeof(ChatWindowsView))
        ];

        private FlyoutMenuItem[] GetProjectOwnerMenuItems() =>
        [
            new("Home", "home.png", typeof(ProjectOwnerHomeView)),
            new("My Profile", "browsepo.png", typeof(ProjectOwnerProfileView)),
            new("Browse Skill Providers", "browsesp.png", typeof(BrowseSkillProvidersView)),
            new("Your Linkages", "linkages.png", typeof(ProjectOwnerOffersAndApplicationsView)),
            new("My Messages", "chat.png", typeof(ChatWindowsView))
        ];

        private FlyoutMenuItem[] GetSkillProviderMenuItems() =>
        [
            new("Home", "home.png", typeof(SkillProviderHomeView)),
            new("My Profile", "browsepo.png", typeof(SkillProviderProfileView)),
            new("Browse Projects", "project.png", typeof(BrowseProjectsView)),
            new("Your Linkages", "linkages.png", typeof(SkillProviderOffersAndApplicationsView)),
            new("My Messages", "chat.png", typeof(ChatWindowsView))
        ];

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(UpdateProfileView), typeof(UpdateProfileView));
            Routing.RegisterRoute("AddProjectView", typeof(AddProjectView));
            Routing.RegisterRoute("UpdateProjectView", typeof(UpdateProjectView));
            Routing.RegisterRoute("ViewProjectView", typeof(ViewProjectView));
            Routing.RegisterRoute(nameof(RateSkillProviderView), typeof(RateSkillProviderView));
            Routing.RegisterRoute(nameof(SendOfferView), typeof(SendOfferView));
            Routing.RegisterRoute("ViewSkillProviderProfileView", typeof(ViewSkillProviderProfileView));
            Routing.RegisterRoute(nameof(AddEducationView), typeof(AddEducationView));
            Routing.RegisterRoute(nameof(UpdateEducationView), typeof(UpdateEducationView));
            Routing.RegisterRoute(nameof(AddSkillView), typeof(AddSkillView));
            Routing.RegisterRoute(nameof(SendApplicationView), typeof(SendApplicationView));
            Routing.RegisterRoute("ViewProjectOwnerProfileView", typeof(ViewProjectOwnerProfileView));
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