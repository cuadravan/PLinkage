using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class RegisterView4 : ContentPage
{
    public RegisterView4(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
