using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid
{
    public partial class UpdateSkillView : ContentPage
    {
        public UpdateSkillView()
        {
            InitializeComponent();
            BindingContext = new UpdateSkillViewModelTemp();
        }
    }
}
