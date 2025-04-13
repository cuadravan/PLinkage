namespace PLinkage.Views;

public partial class SkillProviderProfilePage : ContentPage
{
	public SkillProviderProfilePage()
	{
		InitializeComponent();
        HighlightSidebarButton("profile"); // highlight active tab
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
        await Shell.Current.GoToAsync("///SkillProviderMessagesPage");
    }

    private async void OnUpdateProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SkillUpdateProviderProfilePage");
    }

    private async void OnAddEducationClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///AddEducationPage");
    }

    private async void OnUpdateEducationClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///UpdateEducationPage");
    }

    private async void OnAddSkillClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///AddSkillPage");
    }

    private async void OnUpdateSkillClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///UpdateSkillPage");
    }
}