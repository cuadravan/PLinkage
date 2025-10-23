namespace PLinkageApp.ViewsAndroid;

public partial class AdminBrowseProjectOwnerView : ContentPage
{
    public AdminBrowseProjectOwnerView(AdminBrowseProjectOwnerViewModelTemp adminBrowseProjectOwnerViewModelTemp)
    {
        InitializeComponent();
        BindingContext = adminBrowseProjectOwnerViewModelTemp;

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AdminBrowseProjectOwnerViewModelTemp vm)
            await vm.InitializeAsync();
    }
}