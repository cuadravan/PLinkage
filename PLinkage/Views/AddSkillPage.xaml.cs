namespace PLinkage.Views;

public partial class AddSkillPage : ContentPage
{
	public AddSkillPage()
	{
		InitializeComponent();
	}
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        string skill = skillNameEntry.Text?.Trim();
        string experience = experienceEntry.Text?.Trim();
        string organization = organizationEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(skill) || string.IsNullOrWhiteSpace(experience) || string.IsNullOrWhiteSpace(organization))
        {
            await DisplayAlert("Missing Info", "Please fill in all fields.", "OK");
            return;
        }

        // TODO: Save skill data to backend or local database
        await DisplayAlert("Success", "Skill has been added.", "OK");
        await Shell.Current.GoToAsync("///SkillProviderProfilePage");
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SkillProviderProfilePage");
    }

    private async void OnHomeClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync("///SkillProviderPage");

    private async void OnProfileClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync("///SkillProviderProfilePage");

    private async void OnBrowseClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync("///SkillBrowseProjectPage");

    private async void OnApplicationsClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync("///SkillProviderApplicationsPage");

    private async void OnMessagesClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync("///SkillProviderMessagesPage");
}
