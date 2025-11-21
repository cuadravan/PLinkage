using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class LogoutView : ContentPage
{
	AppShellViewModel appShellViewModel;
	public LogoutView(AppShellViewModel viewModel)
	{
		InitializeComponent();
		appShellViewModel = viewModel;
		_ = Logout();
	}

	private async Task Logout()
	{
		await appShellViewModel.Logout();
	}
}