using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class ProjectOwnerProfileView : ContentPage
{
	public ProjectOwnerProfileView(ProjectOwnerProfileViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProjectOwnerProfileViewModel vm)
            await vm.InitializeAsync();
    }
}