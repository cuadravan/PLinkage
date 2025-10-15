using Microsoft.Maui.Controls;

namespace PLinkageApp.AndroidControls
{
    public partial class StatCard : ContentView
    {
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(
                nameof(Title),
                typeof(string),
                typeof(StatCard),
                string.Empty,
                propertyChanged: OnTitleChanged);

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(
                nameof(Value),
                typeof(string),
                typeof(StatCard),
                string.Empty,
                propertyChanged: OnValueChanged);

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

        private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (StatCard)bindable;
            control.TitleLabel.Text = newValue?.ToString() ?? string.Empty;
        }

        private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (StatCard)bindable;
            control.ValueLabel.Text = newValue?.ToString() ?? string.Empty;
        }
    }
}