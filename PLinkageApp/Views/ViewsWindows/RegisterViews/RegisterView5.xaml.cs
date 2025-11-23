using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class RegisterView5 : ContentPage
{
    public RegisterView5(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
