using Microsoft.Maui.Controls;
using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid
{
    public partial class SentApplicationsPendingView : ContentPage
    {
        public SentApplicationsPendingView()
        {
            InitializeComponent();

            // 🔹 Bind the temporary ViewModel for now
            BindingContext = new SentApplicationsPendingViewModelTemp();
        }
    }
}
