using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class RegisterPage3 : ContentPage
{
    public RegisterPage3()
    {
        InitializeComponent();
        BindingContext = new RegisterPage3ViewModel();
    }
}
