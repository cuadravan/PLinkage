using Microsoft.Maui.Controls;
using PLinkageApp.Behaviors;

namespace PLinkageApp.WindowsControls
{
    public partial class LabeledEntry : ContentView
    {
        public LabeledEntry()
        {
            InitializeComponent();
        }

        // Label Text
        public static readonly BindableProperty LabelTextProperty =
            BindableProperty.Create(nameof(LabelText), typeof(string), typeof(LabeledEntry), string.Empty);

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        // Placeholder
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(LabeledEntry), string.Empty);

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        // Text
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(LabeledEntry), string.Empty, BindingMode.TwoWay);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        // Keyboard
        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(LabeledEntry), Keyboard.Default);

        public Keyboard Keyboard
        {
            get => (Keyboard)GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
        }

        // Vertical Text Alignment
        public static readonly BindableProperty VerticalTextAlignmentProperty =
            BindableProperty.Create(nameof(VerticalTextAlignment), typeof(TextAlignment), typeof(LabeledEntry), TextAlignment.Center);

        public TextAlignment VerticalTextAlignment
        {
            get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
            set => SetValue(VerticalTextAlignmentProperty, value);
        }

        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(LabeledEntry), false);

        public bool IsPassword
        {
            get => (bool)GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        public static readonly BindableProperty IsNumericProperty =
            BindableProperty.Create(
                nameof(IsNumeric),
                typeof(bool),
                typeof(LabeledEntry),
                false, // Default is false (normal text)
                propertyChanged: OnIsNumericChanged); // <--- Hook up the change handler

        public bool IsNumeric
        {
            get => (bool)GetValue(IsNumericProperty);
            set => SetValue(IsNumericProperty, value);
        }

        // 2. Handle the change
        private static void OnIsNumericChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (LabeledEntry)bindable;
            var editor = control.InputEditor;

            // 1. Remove any existing behavior to start fresh
            var existingBehavior = editor.Behaviors.FirstOrDefault(b => b is NumericValidationBehavior);
            if (existingBehavior != null)
            {
                editor.Behaviors.Remove(existingBehavior);
            }

            // 2. If IsNumeric is true, add the behavior with the correct settings
            if (control.IsNumeric)
            {
                var behavior = new NumericValidationBehavior
                {
                    AllowDecimals = control.IsDecimal // Pass the setting down
                };

                editor.Behaviors.Add(behavior);

                if (control.Keyboard == Keyboard.Default)
                {
                    control.Keyboard = Keyboard.Numeric;
                }
            }
        }

        public static readonly BindableProperty IsDecimalProperty =
    BindableProperty.Create(nameof(IsDecimal), typeof(bool), typeof(LabeledEntry), false, propertyChanged: OnIsNumericChanged);
        // Note: We use the SAME changed handler (OnIsNumericChanged) to re-evaluate the behavior

        public bool IsDecimal
        {
            get => (bool)GetValue(IsDecimalProperty);
            set => SetValue(IsDecimalProperty, value);
        }
    }
}
