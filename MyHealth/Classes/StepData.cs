using System;

namespace MyHealth
{
    public class StepData
    {
        public enum StepTypes
        {
            WorkTime,
            ImageSlider,
            FreshStart,
            Seperator
        }
        public StepTypes StepType;

        public string StepName;

        public TimeSpan Duration;

        public enum ImageListes
        {
            Body,Eye
        }
        public ImageListes ImageList;

        public override string ToString()
        {
            switch (StepType)
            {
                case StepTypes.WorkTime:
                    return $"{StepName} | {Duration.ToString("hh':'mm':'ss")}";
                case StepTypes.ImageSlider:
                    return $"{StepName} | ({ImageList}) {Duration.ToString("hh':'mm':'ss")}";
                case StepTypes.FreshStart:
                    return $"{StepName}";
                default:
                    return "";
            }
        }
    }
}
