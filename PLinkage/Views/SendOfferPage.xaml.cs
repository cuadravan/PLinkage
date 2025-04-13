namespace PLinkage.Views;

public partial class SendOfferPage : ContentPage
{
	public SendOfferPage()
	{
		InitializeComponent();

        // Simulated project and provider lists (can be dynamic later)
        ProjectPicker.ItemsSource = new List<string> { "Project A", "Project B", "Project C" };
        ProviderPicker.ItemsSource = new List<string> { "John Doe", "Jane Smith", "Carlos Rivera" };

        // You can pre-select the provider if coming from profile
        // ProviderPicker.SelectedItem = "John Doe";
    }
    private async void OnSendClicked(object sender, EventArgs e)
    {
        string project = ProjectPicker.SelectedItem?.ToString();
        string provider = ProviderPicker.SelectedItem?.ToString();
        string rate = RateEntry.Text;
        string time = TimeframeEntry.Text;

        if (string.IsNullOrWhiteSpace(project) || string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(rate) || string.IsNullOrWhiteSpace(time))
        {
            await DisplayAlert("Error", "Please complete all fields.", "OK");
            return;
        }

        await DisplayAlert("Success", $"Offer sent to {provider}!", "OK");
        await Shell.Current.GoToAsync("///ViewSkillProviderProfilePage");
        // Navigation or backend logic goes here...
    }

    // Sidebar navigation
    private async void OnHomeClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///ProjectOwnerPage");
    private async void OnProfileClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///ProjectOwnerProfilePage");
    private async void OnBrowseClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///BrowseSkillProvidersPage");
    private async void OnApplicationsClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///ApplicationsAndOffersPage");
    private async void OnMessagesClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///SendAOfferPage");
}

