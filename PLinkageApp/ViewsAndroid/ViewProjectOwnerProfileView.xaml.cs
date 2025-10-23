namespace PLinkageApp.ViewsAndroid;

public partial class ViewProjectOwnerProfileView : ContentPage
{
	public ViewProjectOwnerProfileView(ViewProjectOwnerProfileViewModelTemp vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewProjectOwnerProfileViewModelTemp vm)
            await vm.InitializeAsync();
    }
}