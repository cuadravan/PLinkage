using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class AddEducationView : ContentPage
{
	public AddEducationView(AddEducationViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}