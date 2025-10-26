using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class BrowseProjectView : ContentPage
{
	public BrowseProjectView(BrowseProjectViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is BrowseProjectViewModel vm)
            await vm.InitializeAsync();
    }
}