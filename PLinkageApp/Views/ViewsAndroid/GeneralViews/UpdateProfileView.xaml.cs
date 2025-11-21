using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class UpdateProfileView : ContentPage
{
	public UpdateProfileView(UpdateProfileViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UpdateProfileViewModel vm)
            await vm.InitializeAsync();
    }
}