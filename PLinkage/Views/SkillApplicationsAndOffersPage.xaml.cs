namespace PLinkage.Views;

public partial class SkillApplicationsAndOffersPage : ContentPage
{
	public SkillApplicationsAndOffersPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        HighlightSidebarButton("apps"); // Fixed method name casing
    }

    private void HighlightSidebarButton(string activeTab) // Fixed method name casing
    {
        homeBtn.BackgroundColor = activeTab == "home" ? Color.FromArgb("#D0E8FF") : Colors.White;
        profileBtn.BackgroundColor = activeTab == "profile" ? Color.FromArgb("#D0E8FF") : Colors.White;
        browseBtn.BackgroundColor = activeTab == "browse" ? Color.FromArgb("#D0E8FF") : Colors.White;
        appsBtn.BackgroundColor = activeTab == "apps" ? Color.FromArgb("#D0E8FF") : Colors.White;
        messagesBtn.BackgroundColor = activeTab == "messages" ? Color.FromArgb("#D0E8FF") : Colors.White;
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        ProjectOwnerPage.CurrentOwnerTab = "home";
        await Shell.Current.GoToAsync("///SkillProviderPage");
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        ProjectOwnerPage.CurrentOwnerTab = "profile";
        await Shell.Current.GoToAsync("///SkillProviderProfilePage");
    }

    private async void OnBrowseClicked(object sender, EventArgs e)
    {
        ProjectOwnerPage.CurrentOwnerTab = "browse";
        await Shell.Current.GoToAsync("///SkillBrowseProjectPage");
    }

    private async void OnApplicationsClicked(object sender, EventArgs e)
    {
        ProjectOwnerPage.CurrentOwnerTab = "apps";
        await Shell.Current.GoToAsync("///SkillApplicationsAndOffersPage");
    }

    private async void OnMessagesClicked(object sender, EventArgs e)
    {
        ProjectOwnerPage.CurrentOwnerTab = "messages";
        await Shell.Current.GoToAsync("///SkillMessagesPage");
    }
}