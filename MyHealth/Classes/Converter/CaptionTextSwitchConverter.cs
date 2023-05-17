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
            if(values[0] is string stepName)
            {
                if(values[1] is string taskTitle)
                {
                    if (values[2] is StepData.StepTypes stepType)
                        return stepType == StepData.StepTypes.WorkTime ? taskTitle : stepName;
                    else
                        return taskTitle;
                }
                else
                {
                    return stepName;
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
