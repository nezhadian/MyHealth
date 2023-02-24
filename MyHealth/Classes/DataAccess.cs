using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealth
{
    public class DataAccess
    {
        public static StepData[] Steps;

        public static ITimerSlice[] GenerateSteps()
        {
            List<ITimerSlice> steps = new List<ITimerSlice>();
            foreach (StepData item in Steps)
            {
                var step = item.ToStep();
                if (step != null)
                    steps.Add(step);
            }
            return steps.ToArray();
        }

        static DataAccess()
        {
            Steps = Templates.TemplateDictionary["standard"];
        }

    }
}
