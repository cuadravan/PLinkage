using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsWindows;

public partial class SkillProviderLinkagesView : ContentPage
{
	public SkillProviderLinkagesView(SkillProviderLinkagesViewModel viewModel)
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