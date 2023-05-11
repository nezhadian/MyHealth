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
            return value.ToString() == SplitParameterValues(parameter.ToString()).TrueValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = SplitParameterValues(parameter.ToString());

            if (value is bool boolValue)
            {
                return Enum.Parse(targetType, boolValue ? param.TrueValue : param.DefaultValue);
            }
            else
            {
                return Binding.DoNothing;
            }


        }

        (string TrueValue,string DefaultValue) SplitParameterValues(string parameter)
        {
            string[] splited = parameter.Split(',');
            if(splited.Length == 2)
            {
                return (splited[0], splited[1]);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
