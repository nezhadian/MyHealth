using System;
using System.Collections.Generic;

namespace MyHealth
{
    public class Templates
    {
        public static Dictionary<string, StepData[]> TemplateDictionary = new Dictionary<string, StepData[]>()
        {
            {"pomodoro" , new StepData[]{
                        new StepData(){StepName = "25min Work", StepType = StepData.StepTypes.WorkTime,Duration = new TimeSpan(0,25,0)},
                        new StepData(){StepName = "3min Short Break", StepType = StepData.StepTypes.ImageSlider,ImageList =  StepData.ImageListes.Eye,Duration = new TimeSpan(0,3,0)},
                        new StepData(){StepName = "Ready", StepType = StepData.StepTypes.FreshStart},

                        new StepData(){StepType = StepData.StepTypes.Seperator},

                        new StepData(){StepName = "25min Work", StepType = StepData.StepTypes.WorkTime,Duration = new TimeSpan(0,25,0)},
                        new StepData(){StepName = "4min Short Break", StepType = StepData.StepTypes.ImageSlider,ImageList =  StepData.ImageListes.Eye,Duration = new TimeSpan(0,4,0)},
                        new StepData(){StepName = "Ready", StepType = StepData.StepTypes.FreshStart},

                        new StepData(){StepType = StepData.StepTypes.Seperator},

                        new StepData(){StepName = "25min Work", StepType = StepData.StepTypes.WorkTime,Duration = new TimeSpan(0,25,0)},
                        new StepData(){StepName = "5min Short Break", StepType = StepData.StepTypes.ImageSlider,ImageList =  StepData.ImageListes.Eye,Duration = new TimeSpan(0,5,0)},
                        new StepData(){StepName = "Ready", StepType = StepData.StepTypes.FreshStart},

                        new StepData(){StepType = StepData.StepTypes.Seperator},

                        new StepData(){StepName = "25min Work", StepType = StepData.StepTypes.WorkTime,Duration = new TimeSpan(0,25,0)},
                        new StepData(){StepName = "20min Long Break", StepType = StepData.StepTypes.ShortBreak,Duration = new TimeSpan(0,20,0)},
                        new StepData(){StepName = "Ready For New Pomodoro", StepType = StepData.StepTypes.FreshStart},
            }},

            {"standard" , new StepData[]{
                        new StepData(){StepName = "52min Work",StepType = StepData.StepTypes.WorkTime,Duration = new TimeSpan(0,52,0)},
                        new StepData(){StepName = "27min Resting",StepType = StepData.StepTypes.ShortBreak,Duration = new TimeSpan(0,17,0)},
                        new StepData(){StepName = "Ready",StepType = StepData.StepTypes.FreshStart},
                        }},
        };

    }
}
