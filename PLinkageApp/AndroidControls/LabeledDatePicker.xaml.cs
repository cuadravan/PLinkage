using Microsoft.Maui.Controls;

namespace PLinkageApp.AndroidControls
{
    public partial class LabeledDatePicker : ContentView
    {
        public LabeledDatePicker()
        {
            InitializeComponent();
        }

        // 🔹 Label text (binds to the <Label>)
        public static readonly BindableProperty LabelTextProperty =
            BindableProperty.Create(
                nameof(LabelText),
                typeof(string),
                typeof(LabeledDatePicker),
                string.Empty);

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        // 🔹 Date property (binds to the <DatePicker>)
        public static readonly BindableProperty DateProperty =
            BindableProperty.Create(
                nameof(Date),
                typeof(DateTime),
                typeof(LabeledDatePicker),
                DateTime.Today,
                BindingMode.TwoWay);

        public DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }
    }
}
