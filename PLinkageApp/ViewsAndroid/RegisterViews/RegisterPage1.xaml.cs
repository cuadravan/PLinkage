using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class RegisterPage1 : ContentPage
{
    public RegisterPage1()
    {
        InitializeComponent(); // this links to your XAML file
        BindingContext = new RegisterPage1ViewModel();
    }
}
