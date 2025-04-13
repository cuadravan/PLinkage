namespace PLinkage.Views;

public partial class AdminProjectsPage : ContentPage
{
	public AdminProjectsPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

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

    private async void OnProjectTapped(object sender, EventArgs e)
    {
        AdminPage.CurrentAdminTab = "projects";
        await Shell.Current.GoToAsync("///AdminProjectDetailPage");
    }
    private async void OnViewProjectClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///AdminProjectDetailPage");
    }
}