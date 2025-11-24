using PLinkageApp.ViewModels;
namespace PLinkageApp.Views;

public partial class AddSkillView : ContentPage
{
	public AddSkillView(AddSkillViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}