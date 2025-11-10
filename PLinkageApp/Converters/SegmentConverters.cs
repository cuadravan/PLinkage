using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace PLinkageApp.Converters
{
    public class SegmentColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var control = parameter as PLinkageApp.Controls.SegmentedControl;
            return control?.SelectedSegment == (string)value ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#E0E0E0");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null!;
    }

    public class SegmentTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var control = parameter as PLinkageApp.Controls.SegmentedControl;
            return control?.SelectedSegment == (string)value ? Color.FromArgb("#7C4DFF") : Color.FromArgb("#777777");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null!;
    }
}
