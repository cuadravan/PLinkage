using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class SkillProviderOffersAndApplicationsView : ContentPage
{
	public SkillProviderOffersAndApplicationsView(SkillProviderLinkagesViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SkillProviderLinkagesViewModel vm)
            await vm.InitializeAsync();
    }
}