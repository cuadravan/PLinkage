using System.Globalization;

namespace PLinkageApp.Converters
{
    public class BoolToBorderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCurrentUser && isCurrentUser)
            {
                // Return a Brush, not a Color
                return new SolidColorBrush(Colors.MediumPurple);
            }

            // Return a Brush, not a Color
            return new SolidColorBrush(Colors.DodgerBlue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}