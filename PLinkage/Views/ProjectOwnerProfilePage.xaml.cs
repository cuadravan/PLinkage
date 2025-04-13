namespace PLinkage.Views;

public partial class ProjectOwnerProfilePage : ContentPage
{
	public ProjectOwnerProfilePage()
	{
		InitializeComponent();
        SortPicker.ItemsSource = new List<string>
            {
                "Active",
                "Completed",
                "Deactivated"
            };

        SortPicker.SelectedIndex = 0;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        profileBtn.BackgroundColor = ProjectOwnerPage.CurrentOwnerTab == "profile"
            ? Color.FromArgb("#D0E8FF")
            : Colors.White;
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

    private async void OnEditProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///EditProjectOwnerProfilePage");
    }

    private async void OnCreateProjectClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///CreateProjectPage");
    }

    private async void OnViewProjectClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///ViewProjectPage");
    }

    private async void OnUpdateProjectClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///UpdateProjectPage");
    }
}