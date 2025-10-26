using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class AdminBrowseProjectOwnerView : ContentPage
{
    public AdminBrowseProjectOwnerView(AdminBrowseProjectOwnerViewModel adminBrowseProjectOwnerViewModel)
    {
        InitializeComponent();
        BindingContext = adminBrowseProjectOwnerViewModel;

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AdminBrowseProjectOwnerViewModel vm)
            await vm.InitializeAsync();
    }
}