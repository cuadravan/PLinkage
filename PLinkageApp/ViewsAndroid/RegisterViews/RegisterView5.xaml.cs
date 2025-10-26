using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class RegisterView5 : ContentPage
{
    public RegisterView5(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
