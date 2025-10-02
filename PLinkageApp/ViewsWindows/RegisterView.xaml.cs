using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class RegisterView : ContentPage
{
	public RegisterView(RegisterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}