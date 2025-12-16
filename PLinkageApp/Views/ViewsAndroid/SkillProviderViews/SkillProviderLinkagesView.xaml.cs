using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class SkillProviderLinkagesView : ContentPage
{
    private readonly SkillProviderLinkagesViewModel _viewModel;
    public SkillProviderLinkagesView(SkillProviderLinkagesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}