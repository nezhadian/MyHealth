using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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
            List<object> items = new List<object>();
            foreach (StepData item in stepDataList)
            {
                items.Add(item.ToMenuItem());
            }
            return items.ToArray();
        }
    }
}
