using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class RegisterView1 : ContentPage
{
    public RegisterView1(RegisterViewModelTemp viewModelTemp)
    {
        InitializeComponent(); // this links to your XAML file
        BindingContext = viewModelTemp;
    }
}
