using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class SkillProviderAddEducationView : ContentPage
{
	public SkillProviderAddEducationView(AddEducationViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}