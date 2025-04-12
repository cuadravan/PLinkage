namespace PLinkage;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

    private async void OnBtnRegisterButtonClicked(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new RegisterPage());
    }
}