using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class ProjectOwnerAddProjectView : ContentPage
{
	public ProjectOwnerAddProjectView(AddProjectViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}