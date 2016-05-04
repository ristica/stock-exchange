using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace StockExchange.Buyer.Converter
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
            {
                return new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }

            return new SolidColorBrush(Color.FromRgb(0, 100, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
