namespace PLinkage.Views;

public partial class SkillViewProjectPage : ContentPage
{
	public SkillViewProjectPage()
	{
		InitializeComponent();
        LoadPickerValues();
    }
    private void LoadPickerValues()
    {
        PriorityPicker.ItemsSource = new List<string> { "Low", "Medium", "High", "Urgent" };
        StatusPicker.ItemsSource = new List<string> { "Open", "Ongoing", "Paused", "Completed" };
    }

    private void OnPriorityChanged(object sender, EventArgs e)
    {
        var selected = PriorityPicker.SelectedItem?.ToString();
        Console.WriteLine($"Priority selected: {selected}");
        // Optional: Logic when priority changes
    }

    private void OnStatusChanged(object sender, EventArgs e)
    {
        var selected = StatusPicker.SelectedItem?.ToString();
        Console.WriteLine($"Status selected: {selected}");
        // Optional: Logic when status changes
    }

    private async void OnApplyClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///ApplySkillPage");
    }

    private async void OnHomeClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///SkillProviderPage");
    private async void OnProfileClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///SkillProviderProfilePage");
    private async void OnBrowseClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///SkillBrowseProjectPage");
    private async void OnApplicationsClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///SkillApplicationsAndOffersPage");
    private async void OnMessagesClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///SkillProviderMessagesPage");
}

