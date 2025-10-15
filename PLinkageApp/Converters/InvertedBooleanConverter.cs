using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace PLinkageApp.Converters
{
    public class InvertedBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                // Return the opposite of the incoming boolean value
                return !booleanValue;
            }

            // Return the value as is, or false if it's not a boolean (safe fallback)
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Reverse the value again for two-way binding (though rarely needed for IsEnabled)
            if (value is bool booleanValue)
            {
                return !booleanValue;
            }

            // Return false as a safe fallback
            return false;
        }
    }
}