using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace WEMBLEY.DemoApp.ValueConverters
{
    public class ValueToBoolConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                if (parameter is string param)
                {
                    string[] thresholds = param.Split(',');
                    if (thresholds.Length == 2 && long.TryParse(thresholds[0], out long lower) && long.TryParse(thresholds[1], out long upper))
                    {
                        return intValue >= lower && intValue <= upper;
                    }
                }
            }

            return false; // Default value if value is not an int or parameters are invalid
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
