using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MyHealth
{
    class Bool2EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(Enum.TryParse(targetType,parameter.ToString(),out object resault))
            {
                return resault;
            }
            else
            {
                return Binding.DoNothing;
            }
        }
    }
}
