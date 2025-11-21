using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class ProjectOwnerUpdateProjectView : ContentPage
{
	public ProjectOwnerUpdateProjectView(UpdateProjectViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UpdateProjectViewModel vm)
            await vm.InitializeAsync();
    }
}