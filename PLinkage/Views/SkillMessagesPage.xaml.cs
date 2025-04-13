using System;
using Microsoft.Maui.Controls;

namespace PLinkage.Views;

public partial class SkillMessagesPage : ContentPage
{
	public SkillMessagesPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        HighlightTab("messages");
    }

    private void HighlightTab(string tab)
    {
        homeBtn.BackgroundColor = Colors.White;
        profileBtn.BackgroundColor = Colors.White;
        browseBtn.BackgroundColor = Colors.White;
        appsBtn.BackgroundColor = Colors.White;
        messagesBtn.BackgroundColor = Color.FromArgb("#D0E8FF");
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
        // Already here
    }
}