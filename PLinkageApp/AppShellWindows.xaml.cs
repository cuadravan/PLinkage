using Microsoft.Maui.Controls;
using PLinkageApp.Views;
using PLinkageApp.ViewModels;
using PLinkageApp.Interfaces;

namespace PLinkageApp
{
    public partial class AppShellWindows : Shell
    {
        private readonly IStartupService _startupService;
        private bool _initialized;

        public AppShellWindows(AppShellViewModel viewModel, IStartupService startupService)
        {
            InitializeComponent();

            Routing.RegisterRoute("ProjectOwnerUpdateProfileView", typeof(ProjectOwnerUpdateProfileView));
            Routing.RegisterRoute("ProjectOwnerAddProjectView", typeof(ProjectOwnerAddProjectView));
            Routing.RegisterRoute("ProjectOwnerUpdateProjectView", typeof(ProjectOwnerUpdateProjectView));
            Routing.RegisterRoute("ViewProjectView", typeof(ViewProjectView));
            Routing.RegisterRoute("ProjectOwnerRateSkillProviderView", typeof(ProjectOwnerRateSkillProviderView));
            Routing.RegisterRoute("ProjectOwnerSendOfferView", typeof(ProjectOwnerSendOfferView));
            Routing.RegisterRoute("ProjectOwnerSendMessageView", typeof(ProjectOwnerSendMessageView));
            Routing.RegisterRoute("ViewSkillProviderProfileView", typeof(ViewSkillProviderProfileView));
            Routing.RegisterRoute("SkillProviderAddEducationView", typeof(SkillProviderAddEducationView));
            Routing.RegisterRoute("SkillProviderUpdateEducationView", typeof(SkillProviderUpdateEducationView));
            Routing.RegisterRoute("SkillProviderAddSkillView", typeof(SkillProviderAddSkillView));
            Routing.RegisterRoute("SkillProviderUpdateSkillView", typeof(SkillProviderUpdateSkillView));
            Routing.RegisterRoute("SkillProviderSendApplicationView", typeof(SkillProviderSendApplicationView));
            Routing.RegisterRoute("ViewProjectOwnerProfileView", typeof(ViewProjectOwnerProfileView));
            Routing.RegisterRoute(nameof(RegisterView1), typeof(RegisterView1));
            Routing.RegisterRoute(nameof(RegisterView2), typeof(RegisterView2));
            Routing.RegisterRoute(nameof(RegisterView3), typeof(RegisterView3));
            Routing.RegisterRoute(nameof(RegisterView4), typeof(RegisterView4));
            Routing.RegisterRoute(nameof(RegisterView5), typeof(RegisterView5));

            Routing.RegisterRoute(nameof(MessagesView), typeof(MessagesView));
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
