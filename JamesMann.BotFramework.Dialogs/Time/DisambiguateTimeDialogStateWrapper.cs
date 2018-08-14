using JamesMann.BotFramework.Dialogs.Extensions;
using System;
using System.Collections.Generic;

namespace JamesMann.BotFramework.Dialogs.Time
{
    // convenience helper to get/set dialog state
    internal class DisambiguateTimeDialogStateWrapper
    {
        public DisambiguateTimeDialogStateWrapper(IDictionary<string,object> state)
        {
            State = state;
        }

        public IDictionary<string, object> State { get; }

        public RecognizerExtensions.ResolutionList Resolutions {
            get
            {
                return State["recognizedDateTime"] as RecognizerExtensions.ResolutionList;
            }
            set
            {
                State["recognizedDateTime"] = value;
            }
        }

        public TimeSpan Time
        {
            get
            {
                return (TimeSpan)State["time"];
            }
            set
            {
                State["time"] = value;
            }
        }
    }
}
