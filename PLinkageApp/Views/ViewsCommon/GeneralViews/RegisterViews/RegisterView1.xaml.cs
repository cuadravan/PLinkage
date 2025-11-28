using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class RegisterView1 : ContentPage
{
    public RegisterView1(RegisterViewModel viewModel)
    {
        InitializeComponent(); // this links to your XAML file
        BindingContext = viewModel;
    }
}
