namespace PLinkage.Views;

public partial class ViewProjectPage : ContentPage
{
	public ViewProjectPage()
	{
		InitializeComponent();

        // Sample default selections
        PriorityPicker.ItemsSource = new List<string> { "Low", "Medium", "High" };
        StatusPicker.ItemsSource = new List<string> { "Ongoing", "Completed", "Paused" };
    }
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
        await Shell.Current.GoToAsync("///MessagesPage");
    }

    // Picker Change Events
    private void OnPriorityChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedItem != null)
        {
            string selectedPriority = picker.SelectedItem.ToString();
        }
    }

    private void OnStatusChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedItem != null)
        {
            string selectedStatus = picker.SelectedItem.ToString();
        }
    }

    // Remove Button Action (Sample)
    private async void OnRemoveProviderClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Removed", "Skill provider removed.", "OK");
    }
}