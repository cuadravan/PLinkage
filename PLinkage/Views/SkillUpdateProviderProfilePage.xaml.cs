namespace PLinkage.Views;

public partial class SkillUpdateProviderProfilePage : ContentPage
{
	public SkillUpdateProviderProfilePage()
	{
		InitializeComponent();
	}
    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SkillProviderPage");
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SkillProviderProfilePage");
    }

    private async void OnBrowseClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SkillBrowseProjectPage");
    }

    private async void OnApplicationsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SkillApplicationsAndOffersPage");
    }

    private async void OnMessagesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///SkillProviderMessagesPage");
    }

    // Upload Profile Photo (placeholder)
    private async void OnUploadPhotoClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Upload Photo", "Feature not implemented yet.", "OK");
        // Future: MediaPicker or FilePicker logic here
    }

    // Reset all form entries
    private void OnResetClicked(object sender, EventArgs e)
    {
        firstNameEntry.Text = string.Empty;
        lastNameEntry.Text = string.Empty;
        genderEntry.Text = string.Empty;
        locationEntry.Text = string.Empty;
        mobileNumberEntry.Text = string.Empty;
        birthdatePicker.Date = DateTime.Now;
    }

    // Apply Changes (submit form)
    private async void OnApplyChangesClicked(object sender, EventArgs e)
    {
        string firstName = firstNameEntry.Text?.Trim();
        string lastName = lastNameEntry.Text?.Trim();
        string gender = genderEntry.Text?.Trim();
        string location = locationEntry.Text?.Trim();
        string mobile = mobileNumberEntry.Text?.Trim();
        string birthdate = birthdatePicker.Date.ToString("yyyy-MM-dd");

        // Basic validation
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            await DisplayAlert("Missing Info", "Please enter at least First Name and Last Name.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(mobile) || mobile.Length < 10)
        {
            await DisplayAlert("Invalid Input", "Please enter a valid mobile number.", "OK");
            return;
        }

        // TODO: Save data to backend or local DB
        await DisplayAlert("Profile Updated", "Your profile has been successfully updated!", "OK");
    }
}