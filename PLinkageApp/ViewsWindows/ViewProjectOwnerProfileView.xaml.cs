using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class ViewProjectOwnerProfileView : ContentPage
{
	public ViewProjectOwnerProfileView(ViewProjectOwnerProfileViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewProjectOwnerProfileViewModel vm)
             await vm.InitializeAsync();
    }
}