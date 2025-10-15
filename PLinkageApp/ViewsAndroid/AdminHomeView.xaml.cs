namespace PLinkageApp.ViewsAndroid;

public partial class AdminHomeView : ContentPage
{
	private readonly AdminHomeViewModelTemp _viewModel;
    private bool _isInitialized = false;
	public AdminHomeView(AdminHomeViewModelTemp viewModelTemp)
	{
		InitializeComponent();
		BindingContext = viewModelTemp;
		_viewModel = viewModelTemp;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Load data only the first time the page appears
        if (!_isInitialized)
        {
            await _viewModel.InitializeAsync();
            _isInitialized = true;
        }
    }
}