using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class UpdateProjectView : ContentPage
{
	public UpdateProjectView(UpdateProjectViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UpdateProjectViewModel vm)
            await vm.InitializeAsync();
    }
}