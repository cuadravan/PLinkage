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
