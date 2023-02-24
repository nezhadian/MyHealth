using System;

namespace MyHealth
{
    public class WorkTime : ITimerSlice
    {

        public TimeSpan Duration { get; set; }
        public bool RequireClick { get; set; }
        public string StepName { get; set; } = "Work Time";

        public WorkTime(TimeSpan duration)
        {
            Duration = duration;
        }

        public override string ToString()
        {
            return "";
        }
    }
}
