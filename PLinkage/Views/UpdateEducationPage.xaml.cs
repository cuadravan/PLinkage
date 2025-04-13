namespace PLinkage.Views;

public partial class UpdateEducationPage : ContentPage
{
	public UpdateEducationPage()
	{
		InitializeComponent();
        LoadEducationDetails();
    }
    private void LoadEducationDetails()
    {
        // Placeholder values — these can come from a database, API, or binding context
        courseEntry.Text = "BS Computer Science";
        schoolEntry.Text = "USJ-R";
        yearEntry.Text = "2020";
    }

    private async void OnSaveChangesClicked(object sender, EventArgs e)
    {
        string course = courseEntry.Text?.Trim();
        string school = schoolEntry.Text?.Trim();
        string year = yearEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(course) || string.IsNullOrWhiteSpace(school) || string.IsNullOrWhiteSpace(year))
        {
            await DisplayAlert("Missing Info", "All fields are required.", "OK");
            return;
        }

        // TODO: Save updated info to backend/database
        await DisplayAlert("Success", "Education info has been updated.", "OK");
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
