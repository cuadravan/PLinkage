using Microsoft.Maui.Controls;
using System.Globalization; // Required for parsing

namespace PLinkageApp.Behaviors
{
    public class NumericValidationBehavior : Behavior<InputView>
    {
        // Add a property to toggle decimal support
        public bool AllowDecimals { get; set; } = false;

        protected override void OnAttachedTo(InputView bindable)
        {
            bindable.TextChanged += OnTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(InputView bindable)
        {
            bindable.TextChanged -= OnTextChanged;
            base.OnDetachingFrom(bindable);
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // 1. Allow empty input
            if (string.IsNullOrEmpty(e.NewTextValue)) return;

            bool isValid;

            if (AllowDecimals)
            {
                // 2. DECIMAL LOGIC

                // Allow a single decimal point by itself (e.g. user typed ".")
                // or a number ending in a decimal point (e.g. "12.")
                // We let these pass temporarily so the user can finish typing.
                string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                if (e.NewTextValue == decimalSeparator || e.NewTextValue.EndsWith(decimalSeparator))
                {
                    // We only allow this if it doesn't appear twice (e.g. "12..")
                    int count = e.NewTextValue.Count(c => c.ToString() == decimalSeparator);
                    if (count == 1) return;
                }

                // Check if it is a valid double
                isValid = double.TryParse(e.NewTextValue, NumberStyles.Any, CultureInfo.CurrentCulture, out _);
            }
            else
            {
                // 3. INTEGER LOGIC
                isValid = int.TryParse(e.NewTextValue, out _);
            }

            // 4. REJECT INVALID INPUT
            if (!isValid)
            {
                ((InputView)sender).Text = e.OldTextValue;
            }
        }
    }
}