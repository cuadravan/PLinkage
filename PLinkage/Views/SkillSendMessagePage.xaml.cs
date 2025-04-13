namespace PLinkage.Views;

public partial class SkillSendMessagePage : ContentPage
{
	public SkillSendMessagePage()
	{
		InitializeComponent();

        // Example: You can set this dynamically based on selected user
        recipientNameLabel.Text = "Juan Dela Cruz";
    }
    private async void OnSendMessageClicked(object sender, EventArgs e)
    {
        string messageText = messageEditor.Text?.Trim();

        if (string.IsNullOrWhiteSpace(messageText))
        {
            await DisplayAlert("Empty Message", "Please type a message before sending.", "OK");
            return;
        }

        // TODO: Save/send the message to backend service or local database
        await DisplayAlert("Success", "Message has been sent.", "OK");

        await Shell.Current.GoToAsync("///ViewSkillProfilePage");

        messageEditor.Text = string.Empty;
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