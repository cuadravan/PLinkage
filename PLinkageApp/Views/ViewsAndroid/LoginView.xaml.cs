using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}