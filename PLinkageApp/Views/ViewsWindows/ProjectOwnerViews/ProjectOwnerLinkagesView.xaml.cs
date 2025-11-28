using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsWindows;

public partial class ProjectOwnerLinkagesView : ContentPage
{
	public ProjectOwnerLinkagesView(ProjectOwnerLinkagesViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProjectOwnerLinkagesViewModel vm)
            await vm.InitializeAsync();
    }
}