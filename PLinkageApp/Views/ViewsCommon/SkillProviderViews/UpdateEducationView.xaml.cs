using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class UpdateEducationView : ContentPage
{
	public UpdateEducationView(UpdateEducationViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UpdateEducationViewModel vm)
            await vm.InitializeAsync();
    }
}