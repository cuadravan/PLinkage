using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsWindows;

public partial class BrowseSkillProviderView : ContentPage
{
	public BrowseSkillProviderView(BrowseSkillProviderViewModel viewModel)
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