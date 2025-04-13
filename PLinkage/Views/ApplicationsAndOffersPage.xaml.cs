namespace PLinkage.Views;

public partial class ApplicationsAndOffersPage : ContentPage
{
	public ApplicationsAndOffersPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        homeBtn.BackgroundColor = ProjectOwnerPage.CurrentOwnerTab == "home" ? Color.FromArgb("#D0E8FF") : Colors.White;
        profileBtn.BackgroundColor = ProjectOwnerPage.CurrentOwnerTab == "profile" ? Color.FromArgb("#D0E8FF") : Colors.White;
        browseBtn.BackgroundColor = ProjectOwnerPage.CurrentOwnerTab == "browse" ? Color.FromArgb("#D0E8FF") : Colors.White;
        appsBtn.BackgroundColor = ProjectOwnerPage.CurrentOwnerTab == "apps" ? Color.FromArgb("#D0E8FF") : Colors.White;
        messagesBtn.BackgroundColor = ProjectOwnerPage.CurrentOwnerTab == "messages" ? Color.FromArgb("#D0E8FF") : Colors.White;
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        ProjectOwnerPage.CurrentOwnerTab = "home";
        await Shell.Current.GoToAsync("///ProjectOwnerPage");
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        ProjectOwnerPage.CurrentOwnerTab = "profile";
        await Shell.Current.GoToAsync("///ProjectOwnerProfilePage");
    }

    private async void OnBrowseClicked(object sender, EventArgs e)
    {
        ProjectOwnerPage.CurrentOwnerTab = "browse";
        await Shell.Current.GoToAsync("///BrowseSkillProvidersPage");
    }

    private async void OnApplicationsClicked(object sender, EventArgs e)
    {
        ProjectOwnerPage.CurrentOwnerTab = "apps";
        await Shell.Current.GoToAsync("///ApplicationsAndOffersPage");
    }

    private async void OnMessagesClicked(object sender, EventArgs e)
    {
        ProjectOwnerPage.CurrentOwnerTab = "messages";
        await Shell.Current.GoToAsync("///MessagesPage");
    }
}