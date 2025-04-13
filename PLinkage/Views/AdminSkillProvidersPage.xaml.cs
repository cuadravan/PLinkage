namespace PLinkage.Views;

public partial class AdminSkillProvidersPage : ContentPage
{
	public AdminSkillProvidersPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Sidebar highlight logic
        homeFrame.BackgroundColor = AdminPage.CurrentAdminTab == "home" ? Color.FromArgb("#D0E8FF") : Colors.White;
        skillFrame.BackgroundColor = AdminPage.CurrentAdminTab == "skill" ? Color.FromArgb("#D0E8FF") : Colors.White;
        projectFrame.BackgroundColor = AdminPage.CurrentAdminTab == "projects" ? Color.FromArgb("#D0E8FF") : Colors.White;
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        AdminPage.CurrentAdminTab = "home";
        await Shell.Current.GoToAsync("///AdminPage");
    }

    private async void OnBrowseSkillProvidersClicked(object sender, EventArgs e)
    {
        AdminPage.CurrentAdminTab = "skill";
        await Shell.Current.GoToAsync("///AdminSkillProvidersPage");
    }

    private async void OnBrowseProjectsClicked(object sender, EventArgs e)
    {
        AdminPage.CurrentAdminTab = "projects";
        await Shell.Current.GoToAsync("///AdminProjectsPage");
    }

    private async void OnProviderTapped(object sender, EventArgs e)
    {
        AdminPage.CurrentAdminTab = "skill"; // maintain sidebar highlight
        await Shell.Current.GoToAsync("///AdminSkillProviderProfilePage");
    }
}