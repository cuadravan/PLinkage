using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsWindows;

public partial class SkillProviderProfileView : ContentPage
{
	public SkillProviderProfileView(SkillProviderProfileViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SkillProviderProfileViewModel vm)
            await vm.InitializeAsync();
    }
}