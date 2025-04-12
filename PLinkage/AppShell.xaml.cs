using Microsoft.Maui.Controls;
using PLinkage.Views;
using PLinkage.ViewModels;

namespace PLinkage
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));
        }
    }
}
