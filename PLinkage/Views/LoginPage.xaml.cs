namespace PLinkage.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
    private async void OnBtnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///RegisterPage");
    }

    private async void OnBtnLoginClicked(object sender, EventArgs e)
    {
        string email = emailEntry.Text?.Trim();
        string password = passwordEntry.Text;

        if (email == "owner@plinkage.com" && password == "12345")
        {
            await Shell.Current.GoToAsync("///ProjectOwnerPage");
        }
        else if (email == "skill@plinkage.com" && password == "12345")
        {
            await Shell.Current.GoToAsync("///SkillProviderPage");
        }
        else if (email == "admin@plinkage.com" && password == "12345")
        {
            await Shell.Current.GoToAsync("///AdminPage");
        }
        else
        {
            await DisplayAlert("Login Failed", "Invalid credentials", "OK");
        }
    }
}