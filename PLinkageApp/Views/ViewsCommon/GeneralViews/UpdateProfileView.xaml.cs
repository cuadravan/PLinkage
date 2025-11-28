using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class UpdateProfileView : ContentPage
{
	public UpdateProfileView(UpdateProfileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UpdateProfileViewModel vm)
            await vm.InitializeAsync();
    }
}