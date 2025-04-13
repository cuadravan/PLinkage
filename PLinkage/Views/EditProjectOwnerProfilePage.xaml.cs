using System;
using Microsoft.Maui.Controls;

namespace PLinkage.Views;

public partial class EditProjectOwnerProfilePage : ContentPage
{
	public EditProjectOwnerProfilePage()
	{
		InitializeComponent();
	}
    private void OnResetClicked(object sender, EventArgs e)
    {
        firstNameEntry.Text = string.Empty;
        lastNameEntry.Text = string.Empty;
        genderEntry.Text = string.Empty;
        locationEntry.Text = string.Empty;
        birthdateEntry.Text = string.Empty;
        mobileNumberEntry.Text = string.Empty;
    }

    private async void OnApplyChangesClicked(object sender, EventArgs e)
    {
        // Simulate saving changes (replace with actual logic)
        await DisplayAlert("Profile Updated", "Your changes have been saved successfully.", "OK");
    }

    private async void OnUploadPhotoClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Upload", "Photo Uploaded!.", "OK");
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
}
