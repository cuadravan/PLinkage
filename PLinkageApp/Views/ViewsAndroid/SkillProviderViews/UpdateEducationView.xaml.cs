using Microsoft.Maui.Controls;
using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid
{
    public partial class UpdateEducationView : ContentPage
    {
        public UpdateEducationView(UpdateEducationViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is UpdateEducationViewModel vm)
                await vm.InitializeAsync();
        }
    }
}
