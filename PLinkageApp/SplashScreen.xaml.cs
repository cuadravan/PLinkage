using System;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using PLinkageApp.Interfaces;
using PLinkageApp.Views;

namespace PLinkageApp
{
    public partial class SplashScreenPage : ContentPage
    {
        public SplashScreenPage()
        {
            InitializeComponent();
        }

        public async Task RunAnimationAsync()
        {
            await LogoImage.FadeTo(1, 1000);
            await LogoImage.ScaleTo(1.1, 500);
            await LogoImage.ScaleTo(1.0, 300);
            await Tagline.FadeTo(1, 800);
            await Task.Delay(2000);
            await this.FadeTo(0, 400);
        }
    }

}
