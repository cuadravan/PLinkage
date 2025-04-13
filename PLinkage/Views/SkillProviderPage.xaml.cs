namespace PLinkage.Views;

public partial class SkillProviderPage : ContentPage
{
	public SkillProviderPage()
	{
		InitializeComponent();
        HighlightSidebarButton("home");
    }
    private void HighlightSidebarButton(string activeTab)
    {
        homeBtn.BackgroundColor = activeTab == "home" ? Color.FromArgb("#D0E8FF") : Colors.White;
        profileBtn.BackgroundColor = activeTab == "profile" ? Color.FromArgb("#D0E8FF") : Colors.White;
        browseBtn.BackgroundColor = activeTab == "browse" ? Color.FromArgb("#D0E8FF") : Colors.White;
        appsBtn.BackgroundColor = activeTab == "apps" ? Color.FromArgb("#D0E8FF") : Colors.White;
        messagesBtn.BackgroundColor = activeTab == "messages" ? Color.FromArgb("#D0E8FF") : Colors.White;
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        HighlightSidebarButton("home");
        await Shell.Current.GoToAsync("///SkillProviderPage");
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        HighlightSidebarButton("profile");
        await Shell.Current.GoToAsync("///SkillProviderProfilePage");
    }

    private async void OnBrowseClicked(object sender, EventArgs e)
    {
        HighlightSidebarButton("browse");
        await Shell.Current.GoToAsync("///SkillBrowseProjectPage");
    }

    private async void OnApplicationsClicked(object sender, EventArgs e)
    {
        HighlightSidebarButton("apps");
        await Shell.Current.GoToAsync("///SkillApplicationsAndOffersPage");
    }

    private async void OnMessagesClicked(object sender, EventArgs e)
    {
        HighlightSidebarButton("messages");
        await Shell.Current.GoToAsync("///SkillMessagesPage");
    }

    private async void OnViewProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SkillProviderProfilePage");
    }
}