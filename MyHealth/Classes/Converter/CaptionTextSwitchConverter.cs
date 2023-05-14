using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MyHealth
{
    class CaptionTextSwitchConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values[0] is StepData step)
            {
                if(values[1] is string taskTitle)
                {
                    return step.StepType == StepData.StepTypes.WorkTime ? taskTitle : step.StepName;
                }
                else
                {
                    return step.StepName;
                }
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
