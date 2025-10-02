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

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ProjectOwnerProfileViewModel vm)
        {
            vm.OnViewAppearingCommand.Execute(null);
        }
    }

}