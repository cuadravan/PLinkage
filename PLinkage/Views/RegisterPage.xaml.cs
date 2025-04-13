namespace PLinkage.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}
    private async void OnBtnBackToLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///LoginPage");
    }
}