using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsWindows;

public partial class ProjectOwnerHomeView : ContentPage
{
	public ProjectOwnerHomeView(ProjectOwnerHomeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ProjectOwnerHomeViewModel vm)
            await vm.InitializeAsync();
    }

}