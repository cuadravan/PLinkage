namespace PLinkageApp.ViewsAndroid;

public partial class SkillProviderHomeView : ContentPage
{
    private bool _isInitialized = false;
    private readonly SkillProviderHomeViewModelTemp _viewModel;
    public SkillProviderHomeView(SkillProviderHomeViewModelTemp viewModelTemp)
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