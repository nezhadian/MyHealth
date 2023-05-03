using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Linq;
using System.Windows;

namespace MyHealth
{
    public class StepDataCollection 
    {
        public static ITimerSlice[] GenerateSteps(StepData[] stepDataList)
        {
            List<ITimerSlice> steps = new List<ITimerSlice>();
            foreach (StepData item in stepDataList)
            {
                var step = item.ToStep();
                if (step != null)
                    steps.Add(step);
            }
            return steps.ToArray();
        }

        public static object[] GenerateMenuItem(StepData[] stepDataList)
        {
            return stepDataList.Select<StepData, object>((item) => { 
                if (item.StepType == StepData.StepTypes.Seperator)  
                    return new Separator();  
                else  
                    return item;  
            }).ToArray();
        }
    }
}
