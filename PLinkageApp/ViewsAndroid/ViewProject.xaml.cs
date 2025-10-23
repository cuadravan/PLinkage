using PLinkageApp;

namespace PLinkageApp.Views;

public partial class ViewProject : ContentPage
{
    public ViewProject(AdminBrowseProjectViewModelTemp vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
