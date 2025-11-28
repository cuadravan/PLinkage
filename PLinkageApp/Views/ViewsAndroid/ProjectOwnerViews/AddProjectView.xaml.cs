using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class AddProjectView : ContentPage
{
	public AddProjectView(AddProjectViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}