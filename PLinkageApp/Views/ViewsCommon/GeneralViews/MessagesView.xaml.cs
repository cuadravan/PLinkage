using PLinkageApp.ViewModels;

namespace PLinkageApp.Views;

public partial class MessagesView : ContentPage
{
	public MessagesView(MessagesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MessagesViewModel vm)
            await vm.InitializeAsync();
    }
}