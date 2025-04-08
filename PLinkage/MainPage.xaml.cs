namespace PLinkage
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnBtnLoginClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Login", "Login button clicked", "OK");
        }

        private async void OnBtnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
