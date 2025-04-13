namespace PLinkage.Views;

public partial class UpdateProjectPage : ContentPage
{
	public UpdateProjectPage()
	{
		InitializeComponent();

        PriorityPicker.ItemsSource = new List<string> { "Low", "Medium", "High" };
        StatusPicker.ItemsSource = new List<string> { "Active", "Completed", "On Hold" };
        TimeFramePicker.ItemsSource = new List<string> { "1 Week", "2 Weeks", "1 Month", "3 Months" };
        SkillSetPicker.ItemsSource = new List<string> { "Python", "JavaScript", "SQL", "HTML", "CSS" };

        SkillSetPicker.SelectedIndexChanged += OnSkillSelected;
    }
    private void OnSkillSelected(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        var selectedSkill = picker.SelectedItem?.ToString();

        if (!string.IsNullOrWhiteSpace(selectedSkill))
        {
            var tag = new Frame
            {
                Padding = new Thickness(6, 2),
                Margin = new Thickness(4),
                CornerRadius = 6,
                BackgroundColor = Color.FromArgb("#E6E6E6"),
                Content = new Label
                {
                    Text = selectedSkill,
                    FontSize = 12,
                    TextColor = Colors.Black
                }
            };

            SelectedSkillsLayout.Children.Add(tag);
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (StatusPicker.SelectedItem?.ToString() == "Completed")
        {
            await Shell.Current.GoToAsync("///RateSkillProvidersPage");
        }
        else
        {
            await DisplayAlert("Success", "Project changes saved!", "OK");
            await Navigation.PopAsync(); // or any page refresh logic
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // Sidebar Navigation
    private async void OnHomeClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///ProjectOwnerPage");
    private async void OnProfileClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///ProjectOwnerProfilePage");
    private async void OnBrowseClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///BrowseSkillProvidersPage");
    private async void OnApplicationsClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///ApplicationsAndOffersPage");
    private async void OnMessagesClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///MessagesPage");
}

