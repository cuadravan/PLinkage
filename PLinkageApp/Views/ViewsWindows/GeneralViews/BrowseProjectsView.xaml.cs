using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class BrowseProjectsView : ContentPage
{
	public BrowseProjectsView(BrowseProjectViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is BrowseProjectViewModel vm)
            await vm.InitializeAsync();
    }
}