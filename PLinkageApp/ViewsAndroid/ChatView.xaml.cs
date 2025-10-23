namespace PLinkageApp.ViewsAndroid;

public partial class ChatView : ContentPage
{
	public ChatView(ChatViewModelTemp vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ChatViewModelTemp vm)
            await vm.InitializeAsync();
    }
}