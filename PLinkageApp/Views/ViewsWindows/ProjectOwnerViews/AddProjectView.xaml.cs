using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsWindows;

public partial class AddProjectView : ContentPage
{
	public AddProjectView(AddProjectViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}