using PLinkageApp.ViewModels;
namespace PLinkageApp.Views;

public partial class SkillProviderUpdateSkillView : ContentPage
{
	public SkillProviderUpdateSkillView(UpdateSkillViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}