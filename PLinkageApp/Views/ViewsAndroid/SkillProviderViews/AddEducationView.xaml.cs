using Microsoft.Maui.Controls;
using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid
{
    public partial class AddEducationView : ContentPage
    {
        public AddEducationView(AddEducationViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
