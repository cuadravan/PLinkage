using Microsoft.Maui.Controls;

namespace PLinkageApp.WindowsControls
{
    public partial class StatCard : ContentView
    {
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(
                nameof(Title),
                typeof(string),
                typeof(StatCard),
                string.Empty);

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(
                nameof(Value),
                typeof(string),
                typeof(StatCard),
                string.Empty);

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public StatCard()
        {
            InitializeComponent();
        }
    }
}