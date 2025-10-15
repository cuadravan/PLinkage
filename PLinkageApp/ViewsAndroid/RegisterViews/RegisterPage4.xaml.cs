using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class RegisterPage4 : ContentPage
{
    public RegisterPage4()
    {
        InitializeComponent();
        BindingContext = new RegisterPage4ViewModel();
    }
}
