using System;

namespace JamesMann.BotFramework.Dialogs.Extensions
{
    internal class Resolution
    {
        internal enum ResolutionTypes
        {
            Time, DateTime, TimeRange, DateTimeRange, Unknown
        }

        public ResolutionTypes ResolutionType { get; set; }

        public TimeSpan? Time1 { get; set; }
        public TimeSpan? Time2 { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
    }
}
