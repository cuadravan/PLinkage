namespace PLinkage.Views;

public partial class ProjectOwnerPage : ContentPage
{
    public static string CurrentOwnerTab = "home";
    public ProjectOwnerPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        HighlightTab(CurrentOwnerTab);
    }

    private void HighlightTab(string tab)
    {
        homeBtn.BackgroundColor = Colors.White;
        profileBtn.BackgroundColor = Colors.White;
        browseBtn.BackgroundColor = Colors.White;
        appsBtn.BackgroundColor = Colors.White;
        messagesBtn.BackgroundColor = Colors.White;

        switch (tab)
        {
            case "home":
                homeBtn.BackgroundColor = Color.FromArgb("#D0E8FF");
                break;
            case "profile":
                profileBtn.BackgroundColor = Color.FromArgb("#D0E8FF");
                break;
            case "browse":
                browseBtn.BackgroundColor = Color.FromArgb("#D0E8FF");
                break;
            case "apps":
                appsBtn.BackgroundColor = Color.FromArgb("#D0E8FF");
                break;
            case "messages":
                messagesBtn.BackgroundColor = Color.FromArgb("#D0E8FF");
                break;
        }
    }

    private void OnHomeClicked(object sender, EventArgs e)
    {
        CurrentOwnerTab = "home";
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        CurrentOwnerTab = "profile";
        await Shell.Current.GoToAsync("///ProjectOwnerProfilePage");
    }

    private async void OnBrowseSkillProvidersClicked(object sender, EventArgs e)
    {
        CurrentOwnerTab = "browse";
        await Shell.Current.GoToAsync("///BrowseSkillProvidersPage");
    }

    private async void OnApplicationsClicked(object sender, EventArgs e)
    {
        CurrentOwnerTab = "apps";
        await Shell.Current.GoToAsync("///ApplicationsAndOffersPage");
    }

    private async void OnMessagesClicked(object sender, EventArgs e)
    {
        CurrentOwnerTab = "messages";
        await Shell.Current.GoToAsync("///MessagesPage");
    }
}