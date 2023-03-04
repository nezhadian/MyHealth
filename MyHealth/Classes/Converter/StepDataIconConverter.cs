using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Shapes;

namespace MyHealth
{

    class StepDataIconConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            object resource;
            if ((resource = App.Current.TryFindResource($"{values[0]}.{values[1]}")) != null)
                return resource;
            if ((resource = App.Current.TryFindResource($"{values[0]}")) != null)
                return resource;
            return Binding.DoNothing;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
