using Microsoft.Maui.Controls;

namespace PLinkageApp.Controls
{
    public partial class InfoBanner : ContentView
    {
        // Text property
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(InfoBanner),
                string.Empty);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        // IconSource property
        public static readonly BindableProperty IconSourceProperty =
            BindableProperty.Create(
                nameof(IconSource),
                typeof(string),
                typeof(InfoBanner),
                "plinkage.png"); // default icon

        public string IconSource
        {
            get => (string)GetValue(IconSourceProperty);
            set => SetValue(IconSourceProperty, value);
        }

        // BackgroundColor property (optional if you want customizable colors)
        public static readonly BindableProperty BannerBackgroundColorProperty =
            BindableProperty.Create(
                nameof(BannerBackgroundColor),
                typeof(Color),
                typeof(InfoBanner),
                Color.FromArgb("#F2ECFF"));

        public Color BannerBackgroundColor
        {
            get => (Color)GetValue(BannerBackgroundColorProperty);
            set => SetValue(BannerBackgroundColorProperty, value);
        }

        public InfoBanner()
        {
            InitializeComponent();
        }
    }
}
