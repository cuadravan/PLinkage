using Microsoft.Maui.Controls;
using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid
{
    public partial class ApplyView : ContentPage
    {
        public ApplyView(SendApplicationViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is SendApplicationViewModel vm)
                await vm.InitializeAsync();
        }
    }
}
