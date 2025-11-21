using PLinkageApp.ViewModels;

namespace PLinkageApp.ViewsAndroid;

public partial class ProcessResignationView : ContentPage
{
	public ProcessResignationView(ProcessResignationViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProcessResignationViewModel vm)
            await vm.InitializeAsync();
    }
}