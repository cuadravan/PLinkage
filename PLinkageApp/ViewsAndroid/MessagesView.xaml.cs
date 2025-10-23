namespace PLinkageApp.ViewsAndroid;

public partial class MessagesView : ContentPage
{
	public MessagesView(MessagesViewModelTemp vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MessagesViewModelTemp vm)
            await vm.InitializeAsync();
    }
}