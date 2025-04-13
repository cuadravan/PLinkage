namespace PLinkage.Views;

public partial class AdminPage : ContentPage
{
    public static string CurrentAdminTab = "home";
    public AdminPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Highlight the correct tab when on AdminPage (HOME)
        homeFrame.BackgroundColor = CurrentAdminTab == "home" ? Color.FromArgb("#D0E8FF") : Colors.White;
        skillFrame.BackgroundColor = CurrentAdminTab == "skill" ? Color.FromArgb("#D0E8FF") : Colors.White;
        projectFrame.BackgroundColor = CurrentAdminTab == "projects" ? Color.FromArgb("#D0E8FF") : Colors.White;
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        CurrentAdminTab = "home";
        await Shell.Current.GoToAsync("///AdminPage");
    }

    private async void OnBrowseSkillProvidersClicked(object sender, EventArgs e)
    {
        CurrentAdminTab = "skill";
        await Shell.Current.GoToAsync("///AdminSkillProvidersPage");
    }

    private async void OnBrowseProjectsClicked(object sender, EventArgs e)
    {
        CurrentAdminTab = "projects";
        await Shell.Current.GoToAsync("///AdminProjectsPage");
    }
}