using PLinkageApp.ViewModels;
using CommunityToolkit.Mvvm.Input;


namespace PLinkageApp.Views;


public partial class ProjectOwnerProfileView : ContentPage
{
	public ProjectOwnerProfileView(ProjectOwnerProfileViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProjectOwnerProfileViewModel vm)
            await vm.InitializeAsync();
    }

}