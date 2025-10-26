using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid
{
    public partial class AddSkillView : ContentPage
    {
        public AddSkillView()
        {
            InitializeComponent();
            BindingContext = new AddSkillViewModelTemp(); // ✅ Using the temp version
        }
    }
}
