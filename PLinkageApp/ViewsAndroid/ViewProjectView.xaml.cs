namespace PLinkageApp.ViewsAndroid;

public partial class ViewProjectView : ContentPage
{
	public ViewProjectView(ViewProjectViewModelTemp vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewProjectViewModelTemp vm)
            await vm.InitializeAsync();
    }
}