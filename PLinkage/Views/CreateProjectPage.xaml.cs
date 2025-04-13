using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace PLinkage.Views;

public partial class CreateProjectPage : ContentPage
{
	public CreateProjectPage()
	{
		InitializeComponent();
        TimeFramePicker.ItemsSource = new List<string>
            {
                "1 week", "2 weeks", "1 month", "2 months", "3 months"
            };

        // Populate Priority Picker
        PriorityPicker.ItemsSource = new List<string>
            {
                "Low", "Medium", "High", "Critical"
            };

        // Populate Status Picker
        StatusPicker.ItemsSource = new List<string>
            {
                "Planned", "Ongoing", "On Hold", "Completed", "Cancelled"
            };

        // Optional: Set default selections
        TimeFramePicker.SelectedIndex = 0;
        PriorityPicker.SelectedIndex = 1;
        StatusPicker.SelectedIndex = 0;

        // Set Date Created and Date Updated to today
        DateCreatedPicker.Date = DateTime.Today;
        DateUpdatedPicker.Date = DateTime.Today;
    }

    // Navigation Sidebar Button Click Handlers

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

    private async void OnCreateProjectClicked(object sender, EventArgs e)
    {
        // You can add field validation here too
        await DisplayAlert("Success", "Project Created!", "OK");
    }
}
	
