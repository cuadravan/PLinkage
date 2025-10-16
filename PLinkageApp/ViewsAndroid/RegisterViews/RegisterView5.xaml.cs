using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class RegisterView5 : ContentPage
{
    public RegisterView5(RegisterViewModelTemp viewModelTemp)
    {
        InitializeComponent();
        BindingContext = viewModelTemp;
    }
}
