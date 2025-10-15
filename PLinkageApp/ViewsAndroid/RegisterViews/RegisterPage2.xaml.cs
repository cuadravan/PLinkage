using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class RegisterPage2 : ContentPage
{
    public RegisterPage2()
    {
        InitializeComponent();
        BindingContext = new RegisterPage2ViewModel();
    }
}
