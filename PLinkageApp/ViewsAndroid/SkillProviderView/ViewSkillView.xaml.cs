using Microsoft.Maui.Controls;
using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid
{
    public partial class ViewSkillView : ContentPage
    {
        public ViewSkillView(ViewSkillViewModelTemp viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
