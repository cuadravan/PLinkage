using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsWindows
{
    public partial class LoginView : ContentPage
    {
        public LoginView(LoginViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }

}
