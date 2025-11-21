using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class AdminHomeView : ContentPage
{
	private readonly AdminHomeViewModel _viewModel;
    private bool _isInitialized = false;
	public AdminHomeView(AdminHomeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_viewModel = viewModel;
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