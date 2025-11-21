using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class ViewSkillProviderProfileView : ContentPage
{
	public ViewSkillProviderProfileView(ViewSkillProviderProfileViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewSkillProviderProfileViewModel vm)
            await vm.InitializeAsync();
    }
}