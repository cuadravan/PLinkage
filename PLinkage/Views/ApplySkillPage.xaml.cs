namespace PLinkage.Views;

public partial class ApplySkillPage : ContentPage
{
	public ApplySkillPage()
	{
		InitializeComponent();
        LoadPickerData();
    }
    private void LoadPickerData()
    {
        // These can come from a backend or database later
        projectPicker.ItemsSource = new List<string>
        {
            "Web Platform Development",
            "Mobile App Revamp",
            "AI Chatbot Integration"
        };

        providerPicker.ItemsSource = new List<string>
        {
            "Juan Dela Cruz",
            "Maria Lopez",
            "Carlos Reyes"
        };

        // Optional: Pre-select based on context
        projectPicker.SelectedIndex = 0;
        providerPicker.SelectedIndex = 0;
    }

    private async void OnSendClicked(object sender, EventArgs e)
    {
        string selectedProject = projectPicker.SelectedItem?.ToString();
        string selectedProvider = providerPicker.SelectedItem?.ToString();
        string rate = rateEntry.Text?.Trim();
        string timeframe = timeframeEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(selectedProject) ||
            string.IsNullOrWhiteSpace(selectedProvider) ||
            string.IsNullOrWhiteSpace(rate) ||
            string.IsNullOrWhiteSpace(timeframe))
        {
            await DisplayAlert("Missing Info", "Please complete all fields before sending your application.", "OK");
            return;
        }

        // TODO: Save or send application data to backend
        await DisplayAlert("Application Sent", $"You've applied to '{selectedProject}' as '{selectedProvider}'!", "OK");
        await Shell.Current.GoToAsync("///SkillViewProjectPage");
    }

    private async void OnHomeClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///SkillProviderPage");
    private async void OnProfileClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///SkillProviderProfilePage");
    private async void OnBrowseClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///SkillBrowseProjectPage");
    private async void OnApplicationsClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///SkillProviderApplicationsPage");
    private async void OnMessagesClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///SkillProviderMessagesPage");
}

