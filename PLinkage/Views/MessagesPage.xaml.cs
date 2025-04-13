using System;
using Microsoft.Maui.Controls;

namespace PLinkage.Views;

public partial class MessagesPage : ContentPage
{
	public MessagesPage()
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
        // Already here
    }
}