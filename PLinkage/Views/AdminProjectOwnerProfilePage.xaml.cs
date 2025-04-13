namespace PLinkage.Views;

public partial class AdminProjectOwnerProfilePage : ContentPage
{
	public AdminProjectOwnerProfilePage()
	{
		InitializeComponent();
	}
    private void OnHomeClicked(object sender, EventArgs e)
    {
        AdminPage.CurrentAdminTab = "home";
        Shell.Current.GoToAsync("///AdminPage");
    }

    private void OnBrowseSkillProvidersClicked(object sender, EventArgs e)
    {
        AdminPage.CurrentAdminTab = "skill";
        Shell.Current.GoToAsync("///AdminSkillProvidersPage");
    }

    private void OnBrowseProjectsClicked(object sender, EventArgs e)
    {
        AdminPage.CurrentAdminTab = "project";
        Shell.Current.GoToAsync("///AdminProjectsPage");
    }

    private void OnDeactivateClicked(object sender, EventArgs e)
    {
        // Add your deactivate logic here (confirmation, status update, etc.)
        DisplayAlert("Deactivated", "This project owner has been deactivated.", "OK");
    }
}