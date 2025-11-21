using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsWindows;

public partial class BrowseProjectView : ContentPage
{
	public BrowseProjectView(BrowseProjectViewModel viewModel)
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