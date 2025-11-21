using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

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