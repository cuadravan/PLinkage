using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PLinkageShared.DTOs;

namespace PLinkageApp.ViewsAndroid
{
    [QueryProperty(nameof(OwnerId), "OwnerId")]
    public partial class ViewProjectOwnerProfile : ContentPage
    {
        public Guid OwnerId { get; set; }

        public ViewProjectOwnerProfile()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadProfileAsync();
        }

        private async Task LoadProfileAsync()
        {
            await Task.Delay(300); // simulate API call

            // Dummy profile data
            OwnerNameLabel.Text = "Van Cuadra";
            LocationLabel.Text = "Carmen";
            JoinedDateLabel.Text = "Joined on: April 24, 2025";
            GenderLabel.Text = "Male";
            EmailLabel.Text = "cuadra@gmail.com";
            MobileLabel.Text = "09123456789";

            // Dummy project list
            var projects = new ObservableCollection<ProjectDisplayItem>
            {
                new ProjectDisplayItem { Title = "Happy Fishing", Description = "Let's go fishing!", Status = "Status: Completed", StatusColor = Colors.Green },
                new ProjectDisplayItem { Title = "Book Selling", Description = "Let's sell books!", Status = "Status: Active", StatusColor = Colors.Blue },
                new ProjectDisplayItem { Title = "Car Restoration", Description = "Restoring the old car.", Status = "Status: Deactivated", StatusColor = Colors.Red }
            };

            // **Important**: assign to the named CollectionView
            ProjectsCollection.ItemsSource = projects;
        }

        public class ProjectDisplayItem
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public Color StatusColor { get; set; }
        }
    }
}
