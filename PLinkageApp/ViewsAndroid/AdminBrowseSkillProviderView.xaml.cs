namespace PLinkageApp.ViewsAndroid;

public partial class AdminBrowseSkillProviderView : ContentPage
{
	public AdminBrowseSkillProviderView(AdminBrowseSkillProviderViewModelTemp viewModelTemp)
	{
		InitializeComponent();
		BindingContext = viewModelTemp;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is AdminBrowseSkillProviderViewModelTemp vm)
			await vm.InitializeAsync();
	}
}