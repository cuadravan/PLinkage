using Microsoft.Maui.Controls;
using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid
{
    public partial class NegotiatingOfferView : ContentPage
    {
        public NegotiatingOfferView(NegotiateViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is NegotiateViewModel vm)
                await vm.InitializeAsync();
        }
    }
}
