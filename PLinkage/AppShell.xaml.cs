using Microsoft.Maui.Controls;
using PLinkage.Views;
using PLinkage.ViewModels;
using PLinkage.Interfaces;

namespace PLinkage
{
    public partial class AppShell : Shell
    {
        private readonly IStartupService _startupService;
        private bool _initialized;

        public AppShell(AppShellViewModel viewModel, IStartupService startupService)
        {
            InitializeComponent();

            Routing.RegisterRoute("ProjectOwnerUpdateProfileView", typeof(ProjectOwnerUpdateProfileView));
            Routing.RegisterRoute("ProjectOwnerAddProjectView", typeof(ProjectOwnerAddProjectView));
            Routing.RegisterRoute("ProjectOwnerUpdateProjectView", typeof(ProjectOwnerUpdateProjectView));
            Routing.RegisterRoute("ProjectOwnerViewProjectView", typeof(ProjectOwnerViewProjectView));
            Routing.RegisterRoute("ProjectOwnerRateSkillProviderView", typeof(ProjectOwnerRateSkillProviderView));
            Routing.RegisterRoute("ProjectOwnerSendOfferView", typeof(ProjectOwnerSendOfferView));
            Routing.RegisterRoute("ProjectOwnerSendMessageView", typeof(ProjectOwnerSendMessageView));
            Routing.RegisterRoute("ProjectOwnerViewSkillProviderProfileView", typeof(ProjectOwnerViewSkillProviderProfileView));
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
    }
}
