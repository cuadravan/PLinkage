using Microsoft.Maui.Controls;

namespace PLinkageApp.ViewsAndroid
{
    public partial class SentApplicationsHistoryView : ContentPage
    {
        public SentApplicationsHistoryView()
        {
            InitializeComponent();
            BindingContext = new SentApplicationsHistoryViewModelTemp();
        }
    }
}
