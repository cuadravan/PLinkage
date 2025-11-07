using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class SkillProviderLinkagesView : ContentPage
{
	public SkillProviderLinkagesView(SkillProviderLinkagesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SkillProviderLinkagesViewModel vm)
            await vm.InitializeAsync();
    }
}