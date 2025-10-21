namespace PLinkageApp.ViewsAndroid;

public partial class AdminBrowseProjectView : ContentPage
{
	public AdminBrowseProjectView(AdminBrowseProjectViewModelTemp adminBrowseProjectViewModelTemp)
	{
		InitializeComponent();
		BindingContext = adminBrowseProjectViewModelTemp;

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AdminBrowseProjectViewModelTemp vm)
            await vm.InitializeAsync();
    }
}