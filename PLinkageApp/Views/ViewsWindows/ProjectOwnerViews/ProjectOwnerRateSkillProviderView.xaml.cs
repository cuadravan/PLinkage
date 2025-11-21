using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class ProjectOwnerRateSkillProviderView : ContentPage
{
	public ProjectOwnerRateSkillProviderView(RateSkillProviderViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is RateSkillProviderViewModel vm)
            await vm.InitializeAsync();
    }
}