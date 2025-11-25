using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsWindows;

public partial class ViewSkillProviderProfileView : ContentPage
{
	public ViewSkillProviderProfileView(ViewSkillProviderProfileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewSkillProviderProfileViewModel vm)
            await vm.InitializeAsync();
    }
}