using PLinkage.ViewModels;

namespace PLinkage.ViewsAndroid;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}