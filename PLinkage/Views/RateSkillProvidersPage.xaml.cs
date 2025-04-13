namespace PLinkage.Views;

public partial class RateSkillProvidersPage : ContentPage
{
	public RateSkillProvidersPage()
	{
		InitializeComponent();
	}
    private async void OnSubmitRatingClicked(object sender, EventArgs e)
    {
        // Simulate save logic
        await DisplayAlert("Submitted", "Skill provider ratings have been submitted.", "OK");
        await Shell.Current.GoToAsync("///UpdateProjectPage"); // Navigate back or update based on your flow
    }

    // Sidebar Navigation Handlers
    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///ProjectOwnerPage");
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///ProjectOwnerProfilePage");
    }

    private async void OnBrowseClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///BrowseSkillProvidersPage");
    }

    private async void OnApplicationsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///ApplicationsAndOffersPage");
    }

    private async void OnMessagesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SkillMessagesPage");
    }
}