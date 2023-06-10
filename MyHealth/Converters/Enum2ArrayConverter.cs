using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MyHealth
{

    class Enum2ArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Type baseType)
            {
                Type targetEnumType = baseType.GetNestedType(parameter.ToString());

                if (targetEnumType == null || !targetEnumType.IsEnum)
                    return Binding.DoNothing;

                return targetEnumType.GetEnumValues();
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
