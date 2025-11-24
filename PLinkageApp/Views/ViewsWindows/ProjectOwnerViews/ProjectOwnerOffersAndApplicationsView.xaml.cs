using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class ProjectOwnerOffersAndApplicationsView : ContentPage
{
	public ProjectOwnerOffersAndApplicationsView(ProjectOwnerLinkagesViewModel viewModel)
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