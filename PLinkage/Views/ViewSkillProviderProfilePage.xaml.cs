namespace PLinkage.Views;

public partial class ViewSkillProviderProfilePage : ContentPage
{
	public ViewSkillProviderProfilePage()
	{
		InitializeComponent();
	}

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///ProjectOwnerPage");
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///ProjectOwnerProfilePage");
    }

    private async void OnBrowseClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///BrowseSkillProvidersPage");
    }

    private async void OnApplicationsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///ApplicationsAndOffersPage");
    }

    private async void OnMessagesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MessagesPage");

    }

    private async void OnSendOfferClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SendOfferPage");
    }
    private async void OnMessageClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SendMessagePage");
    }
}