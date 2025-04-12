using System;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using PLinkage.Views;

namespace PLinkage
{
    public partial class SplashScreenPage : ContentPage
    {
        public SplashScreenPage()
        {
            InitializeComponent();
            AnimateSplash();
        }

        private async void AnimateSplash()
        {
            await LogoImage.FadeTo(1, 1000);
            await LogoImage.ScaleTo(1.1, 500);
            await LogoImage.ScaleTo(1.0, 300);
            await Tagline.FadeTo(1, 800);
            await Task.Delay(2000);
            await this.FadeTo(0, 400); // fade out splash

            Application.Current.MainPage = App.ServiceProvider.GetRequiredService<AppShell>();

            await Task.Delay(100);

            await Shell.Current.Navigation.PushAsync(App.ServiceProvider.GetRequiredService<LoginView>());


        }
    }
}
