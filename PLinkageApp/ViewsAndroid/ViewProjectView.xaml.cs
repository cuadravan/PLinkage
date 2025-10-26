using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class ViewProjectView : ContentPage
{
	public ViewProjectView(ViewProjectViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewProjectViewModel vm)
            await vm.InitializeAsync();
    }
}