using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class RegisterView2 : ContentPage
{
    public RegisterView2(RegisterViewModelTemp viewModelTemp)
    {
        InitializeComponent();
        BindingContext = viewModelTemp;
    }
}
