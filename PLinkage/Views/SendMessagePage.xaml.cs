namespace PLinkage.Views;

public partial class SendMessagePage : ContentPage
{
	public SendMessagePage()
	{
		InitializeComponent();
	}
    private async void OnSendMessageClicked(object sender, EventArgs e)
    {
        string recipient = RecipientNameEntry.Text?.Trim();
        string message = MessageEditor.Text?.Trim();

        if (string.IsNullOrWhiteSpace(recipient) || string.IsNullOrWhiteSpace(message))
        {
            await DisplayAlert("Incomplete", "Please fill in both the recipient name and your message.", "OK");
            return;
        }

        // Simulated send logic
        await DisplayAlert("Message Sent", $"Your message to {recipient} has been sent successfully!", "OK");
        await Shell.Current.GoToAsync("///ViewSkillProviderProfilePage");

        // Optional: clear fields
        RecipientNameEntry.Text = string.Empty;
        MessageEditor.Text = string.Empty;
    }

    // Sidebar Navigation
    private async void OnHomeClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("///ProjectOwnerPage");

    private async void OnProfileClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("///ProjectOwnerProfilePage");

    private async void OnBrowseClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("///BrowseSkillProvidersPage");

    private async void OnApplicationsClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("///ApplicationsAndOffersPage");

    private async void OnMessagesClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("///SkillMessagesPage");
}

