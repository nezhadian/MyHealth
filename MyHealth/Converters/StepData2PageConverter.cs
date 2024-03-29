﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MyHealth
{
    class StepData2PageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StepData step)
            {
                switch (step.StepType)
                {
                    case StepData.StepTypes.ImageSlider:
                        return new PictureSlideShowStep(step);
                    case StepData.StepTypes.FreshStart:
                        return new FreshStartStep() ;
                    case StepData.StepTypes.ShortBreak:
                        return new FreeTimeStep(step);
                    default:
                        return null;
                }
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
