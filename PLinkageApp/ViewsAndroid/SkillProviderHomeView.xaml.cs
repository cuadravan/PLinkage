namespace PLinkageApp.ViewsAndroid;

public partial class SkillProviderHomeView : ContentPage
{
	public SkillProviderHomeView(SkillProviderHomeViewModelTemp viewModelTemp)
	{
		InitializeComponent();
		BindingContext = viewModelTemp;
	}
}