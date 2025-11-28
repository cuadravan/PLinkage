using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsWindows;

public partial class UpdateProjectView : ContentPage
{
	public UpdateProjectView(UpdateProjectViewModel viewModel)
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