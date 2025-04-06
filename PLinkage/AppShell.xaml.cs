using Microsoft.Maui.Controls;
using PLinkage.ApplicationLayer.ViewModels;

namespace PLinkage
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
