using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class BrowseSkillProvidersView : ContentPage
{
	public BrowseSkillProvidersView(BrowseSkillProviderViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is BrowseSkillProviderViewModel vm)
            await vm.InitializeAsync();
    }
}