using System;

namespace MyHealth
{
    public class StepData
    {
        public enum StepTypes
        {
            WorkTime,
            ImageSlider,
            ShortBreak,
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
                case StepTypes.ShortBreak:
                    return $"{StepName} | {Duration.ToString("hh':'mm':'ss")}";
                case StepTypes.ImageSlider:
                    return $"{StepName} | ({ImageList}) {Duration.ToString("hh':'mm':'ss")}";
                case StepTypes.FreshStart:
                    return $"{StepName}";
                default:
                    return "";
            }
        }

        internal ITimerSlice ToStep()
        {
            switch (StepType)
            {
                case StepTypes.WorkTime:
                    return new WorkTime(Duration);
                case StepTypes.ImageSlider:
                    switch (ImageList)
                    {
                        case ImageListes.Body:
                            return new ImageSlider(Duration, System.IO.Path.Combine(Environment.CurrentDirectory, "Images", "body")) { StepName = StepName };
                        case ImageListes.Eye:
                            return new ImageSlider(Duration, System.IO.Path.Combine(Environment.CurrentDirectory, "Images", "eye")) { StepName = StepName };
                    }
                    return null;
                case StepTypes.FreshStart:
                    return new FreshStart() { StepName = StepName };
                case StepTypes.ShortBreak:
                    return new ShortBreak(Duration) { StepName = StepName };
                default:
                    return null;
            }
        }
    }
}
