namespace PLinkageApp.ViewsAndroid;

public partial class ViewSkillProviderProfileView : ContentPage
{
	public ViewSkillProviderProfileView(ViewSkillProviderProfileViewModelTemp vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewSkillProviderProfileViewModelTemp vm)
            await vm.InitializeAsync();
    }
}