using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace PLinkageApp.ViewModels
{
    public partial class SentApplicationsPendingViewModelTemp : ObservableObject
    {
        public SentApplicationsPendingViewModelTemp()
        {
            mainSegments = new ObservableCollection<string> { "Sent Applications", "Received Offers" };
            subSegments = new ObservableCollection<string> { "Pending", "History" };
            selectedMainSegment = "Sent Applications";
            selectedSubSegment = "Pending";
        }

        [ObservableProperty] private ObservableCollection<string> mainSegments;
        [ObservableProperty] private ObservableCollection<string> subSegments;
        [ObservableProperty] private string selectedMainSegment;
        [ObservableProperty] private string selectedSubSegment;
    }
}
