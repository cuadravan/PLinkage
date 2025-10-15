using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class RegisterPage5 : ContentPage
{
    public RegisterPage5()
    {
        InitializeComponent();
        BindingContext = new RegisterPage5ViewModel();
    }
}
