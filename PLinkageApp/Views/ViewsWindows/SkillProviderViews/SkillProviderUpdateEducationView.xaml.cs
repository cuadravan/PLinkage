using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class SkillProviderUpdateEducationView : ContentPage
{
	public SkillProviderUpdateEducationView(UpdateEducationViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}