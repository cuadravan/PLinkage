using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class ChatWindowsView : ContentPage
{
	public ChatWindowsView(ChatWindowsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ChatWindowsViewModel vm)
            await vm.InitializeAsync();
    }
}