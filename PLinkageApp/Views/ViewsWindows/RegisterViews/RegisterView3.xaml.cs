using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class RegisterView3 : ContentPage
{
    public RegisterView3(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
