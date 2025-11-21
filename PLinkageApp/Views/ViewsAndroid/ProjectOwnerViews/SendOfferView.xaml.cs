using Microsoft.Maui.Controls;
using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid
{
    public partial class SendOfferView : ContentPage
    {
        public SendOfferView(SendOfferViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is SendOfferViewModel vm)
                await vm.InitializeAsync();
        }
    }
}
