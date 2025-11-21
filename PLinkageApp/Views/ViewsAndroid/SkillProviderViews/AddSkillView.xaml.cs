using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid
{
    public partial class AddSkillView : ContentPage
    {
        public AddSkillView(AddSkillViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
