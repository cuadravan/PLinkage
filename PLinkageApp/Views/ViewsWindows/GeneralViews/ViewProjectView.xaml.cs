using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsWindows;

public partial class ViewProjectView : ContentPage
{
	public ViewProjectView(ViewProjectViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewProjectViewModel vm)
            await vm.InitializeAsync();
    }
}