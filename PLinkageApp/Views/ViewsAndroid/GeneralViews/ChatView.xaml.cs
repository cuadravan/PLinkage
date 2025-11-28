using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class ChatView : ContentPage
{
	public ChatView(ChatViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ChatViewModel vm)
            await vm.InitializeAsync();
    }
}