using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsWindows;

public partial class AdminHomeView : ContentPage
{
	public AdminHomeView(AdminHomeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AdminHomeViewModel vm)
            await vm.InitializeAsync();
    }
}