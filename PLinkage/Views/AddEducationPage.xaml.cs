namespace PLinkage.Views;

public partial class AddEducationPage : ContentPage
{
	public AddEducationPage()
	{
		InitializeComponent();
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

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        string course = courseEntry.Text?.Trim();
        string school = schoolEntry.Text?.Trim();
        string year = yearGraduatedEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(course) || string.IsNullOrWhiteSpace(school) || string.IsNullOrWhiteSpace(year))
        {
            await DisplayAlert("Missing Info", "Please fill out all fields.", "OK");
            return;
        }

        // TODO: Save education data to database or backend
        await DisplayAlert("Success", "Education has been added.", "OK");
        await Shell.Current.GoToAsync("///SkillProviderProfilePage"); // or any back page
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SkillProviderProfilePage"); // or back to education list
    }
}