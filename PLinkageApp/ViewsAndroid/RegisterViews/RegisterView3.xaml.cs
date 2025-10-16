using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class RegisterView3 : ContentPage
{
    public RegisterView3(RegisterViewModelTemp viewModelTemp)
    {
        InitializeComponent();
        BindingContext = viewModelTemp;
    }
}
