using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using WEMBLEY.DemoApp.Helpers;

namespace WEMBLEY.DemoApp.ValueConverters;
[ValueConversion(typeof(Enum), typeof(string))]
public class EnumToStringConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return EnumHelper.Description((Enum)value);
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
