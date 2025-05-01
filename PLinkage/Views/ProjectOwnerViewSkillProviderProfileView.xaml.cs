using PLinkage.ViewModels;

namespace PLinkage.Views;

public partial class ProjectOwnerViewSkillProviderProfileView : ContentPage
{
	public ProjectOwnerViewSkillProviderProfileView(SkillProviderProfileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SkillProviderProfileViewModel vm)
            await vm.OnViewAppearingCommand.ExecuteAsync(null);
    }
}