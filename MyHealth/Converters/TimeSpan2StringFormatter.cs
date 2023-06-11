using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MyHealth
{
    class TimeSpan2StringFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan)
            {
                var timeSpan = (TimeSpan)value;
                if (timeSpan.Hours > 0)
                    return timeSpan.ToString("hh':'mm':'ss");
                else if (timeSpan.Minutes > 0 || timeSpan.Seconds > 0)
                    return timeSpan.ToString("mm':'ss");
                else
                    return "No Time";
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
