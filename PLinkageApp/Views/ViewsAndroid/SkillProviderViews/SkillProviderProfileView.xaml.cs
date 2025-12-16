using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class SkillProviderProfileView : ContentPage
{
    public SkillProviderProfileView(SkillProviderProfileViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SkillProviderProfileViewModel vm)
            await vm.InitializeAsync();
    }
}