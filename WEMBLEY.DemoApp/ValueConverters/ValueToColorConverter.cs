using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace WEMBLEY.DemoApp.ValueConverters
{
    public class ValueToColorConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            long cellValue = (long)values[0];
            long min = (long)values[1];
            long max = (long)values[2];

            if (cellValue > min && cellValue < max)
                return new SolidColorBrush(Colors.Green);
            else
                return new SolidColorBrush(Colors.Red);

            return new SolidColorBrush(Colors.Transparent); // Default color if values are invalid
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
