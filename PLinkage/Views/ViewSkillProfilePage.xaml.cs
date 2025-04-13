namespace PLinkage.Views;

public partial class ViewSkillProfilePage : ContentPage
{
	public ViewSkillProfilePage()
	{
		InitializeComponent();
	}
    private void OnSortOptionChanged(object sender, EventArgs e)
    {
        var selectedOption = sortPicker.SelectedItem?.ToString();

        switch (selectedOption)
        {
            case "Newest First":
                // Sort logic placeholder
                Console.WriteLine("Sorting: Newest First");
                break;
            case "Oldest First":
                Console.WriteLine("Sorting: Oldest First");
                break;
            case "Status: Ongoing":
                Console.WriteLine("Filtering: Ongoing Projects");
                break;
            case "Status: Completed":
                Console.WriteLine("Filtering: Completed Projects");
                break;
        }

        // Optional: Add logic to refresh or rebind the project cards
    }

    private async void OnHomeClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync("///SkillProviderPage");

    private async void OnProfileClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync("///SkillProviderProfilePage");

    private async void OnBrowseClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync("///SkillBrowseProjectPage");

    private async void OnApplicationsClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync("///SkillApplicationsAndOffersPage");

    private async void OnMessagesClicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync("///SkillMessagesPage");

    private async void OnMessageClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SkillSendMessagePage");
    }

    private async void OnViewProjectClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SkillViewProjectPage");
    }
}

