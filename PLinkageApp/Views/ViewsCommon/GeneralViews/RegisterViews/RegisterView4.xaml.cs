using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class RegisterView4 : ContentPage
{
    public RegisterView4(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
