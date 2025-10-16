using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class RegisterView4 : ContentPage
{
    public RegisterView4(RegisterViewModelTemp viewModelTemp)
    {
        InitializeComponent();
        BindingContext = viewModelTemp;
    }
}
