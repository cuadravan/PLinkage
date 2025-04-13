namespace PLinkage.Views;

public partial class UpdateSkillPage : ContentPage
{
	public UpdateSkillPage()
	{
		InitializeComponent();
        LoadExistingSkill(); // Pre-fill for update
    }
    private void LoadExistingSkill()
    {
        // Sample data — in real app, bind actual skill record
        skillNameEntry.Text = "Web Development";
        experienceEntry.Text = "3";
        organizationEntry.Text = "Freelance / Upwork";
    }

    private async void OnSaveChangesClicked(object sender, EventArgs e)
    {
        string skill = skillNameEntry.Text?.Trim();
        string experience = experienceEntry.Text?.Trim();
        string organization = organizationEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(skill) || string.IsNullOrWhiteSpace(experience) || string.IsNullOrWhiteSpace(organization))
        {
            await DisplayAlert("Missing Info", "Please fill in all fields.", "OK");
            return;
        }

        // TODO: Update skill info in database/backend
        await DisplayAlert("Success", "Skill has been updated.", "OK");
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
