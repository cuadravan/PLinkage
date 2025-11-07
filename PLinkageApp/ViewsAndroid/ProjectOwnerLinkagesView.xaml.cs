using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class ProjectOwnerLinkagesView : ContentPage
{
	public ProjectOwnerLinkagesView(ProjectOwnerLinkagesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProjectOwnerLinkagesViewModel vm)
            await vm.InitializeAsync();
    }
}