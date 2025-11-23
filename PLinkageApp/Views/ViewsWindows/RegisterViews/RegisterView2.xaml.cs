using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class RegisterView2 : ContentPage
{
    public RegisterView2(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
