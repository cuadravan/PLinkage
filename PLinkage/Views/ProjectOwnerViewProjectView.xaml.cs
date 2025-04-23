using PLinkage.ViewModels;

namespace PLinkage.Views;

public partial class ProjectOwnerViewProjectView : ContentPage
{
	public ProjectOwnerViewProjectView(ViewProjectViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ViewProjectViewModel vm)
            await vm.OnAppearingCommand.ExecuteAsync(null);
    }
}