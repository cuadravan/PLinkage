using PLinkageApp.ViewModels;
namespace PLinkageApp.Views;

public partial class SkillProviderAddSkillView : ContentPage
{
	public SkillProviderAddSkillView(AddSkillViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}