using Microsoft.Maui.Controls;
using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid
{
    public partial class ViewSkillView : ContentPage
    {
        public ViewSkillView(ViewSkillViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ViewSkillViewModel vm)
                await vm.InitializeAsync();
        }
    }
}
