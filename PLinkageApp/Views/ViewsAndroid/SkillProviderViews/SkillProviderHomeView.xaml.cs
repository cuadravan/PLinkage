using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class SkillProviderHomeView : ContentPage
{
    private bool _isInitialized = false;
    private readonly SkillProviderHomeViewModel _viewModel;
    public SkillProviderHomeView(SkillProviderHomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_isInitialized)
        {
            await _viewModel.InitializeAsync();
            _isInitialized = true;
        }
    }
}