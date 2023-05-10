using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Linq;

namespace MyHealth
{
    class JoinedTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value.ToString();
            StringBuilder formatted = new StringBuilder();
            foreach (char ch in text)
            {
                formatted.Append(char.IsUpper(ch) ? " " + ch : ch.ToString());
            }
            return formatted.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
