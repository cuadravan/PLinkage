using Microsoft.Maui.Controls;
using PLinkageApp.Views;
using PLinkageApp.ViewModels;
using PLinkageApp.Interfaces;
using PLinkageApp.ViewsAndroid; // ? must match your RegisterPage namespaces

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
            Routing.RegisterRoute(nameof(RegisterPage1), typeof(RegisterPage1));
            Routing.RegisterRoute(nameof(RegisterPage2), typeof(RegisterPage2));
            Routing.RegisterRoute(nameof(RegisterPage3), typeof(RegisterPage3));
            Routing.RegisterRoute(nameof(RegisterPage4), typeof(RegisterPage4));
            Routing.RegisterRoute(nameof(RegisterPage5), typeof(RegisterPage5));
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
