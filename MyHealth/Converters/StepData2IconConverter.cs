using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Shapes;

namespace MyHealth
{

    class StepData2IconConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values[0] is StepData.StepTypes stepType)
            {
                if(stepType != StepData.StepTypes.ImageSlider)
                {
                    return App.Current.Resources[$"Icons.StepData.{stepType}"];
                }
                else
                {
                    if (values[1] is StepData.ImageListes imageList)
                    {
                        return App.Current.Resources[$"Icons.StepData.{stepType}-{imageList}"];

                    }
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
