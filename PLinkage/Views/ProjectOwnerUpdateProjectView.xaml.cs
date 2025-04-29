using PLinkage.ViewModels;

namespace PLinkage.Views;

public partial class ProjectOwnerUpdateProjectView : ContentPage
{
	public ProjectOwnerUpdateProjectView(UpdateProjectViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}