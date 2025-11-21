using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class RateSkillProviderView : ContentPage
{
	public RateSkillProviderView(RateSkillProviderViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is RateSkillProviderViewModel vm)
            await vm.InitializeAsync();
    }
}