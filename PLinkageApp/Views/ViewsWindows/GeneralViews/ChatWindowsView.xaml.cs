using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsWindows;

public partial class ChatWindowsView : ContentPage
{
	public ChatWindowsView(ChatViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ChatViewModel vm)
            await vm.InitializeAsync();
    }
}