using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class ResignProjectView : ContentPage
{
	public ResignProjectView(ResignProjectViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ResignProjectViewModel vm)
            await vm.InitializeAsync();
    }
}