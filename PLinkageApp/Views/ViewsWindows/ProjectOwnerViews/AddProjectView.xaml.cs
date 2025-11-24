using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class AddProjectView : ContentPage
{
	public AddProjectView(AddProjectViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}