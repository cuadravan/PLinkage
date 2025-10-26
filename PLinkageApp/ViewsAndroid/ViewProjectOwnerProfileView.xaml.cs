using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class ViewProjectOwnerProfileView : ContentPage
{
	public ViewProjectOwnerProfileView(ViewProjectOwnerProfileViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewProjectOwnerProfileViewModel vm)
            await vm.InitializeAsync();
    }
}