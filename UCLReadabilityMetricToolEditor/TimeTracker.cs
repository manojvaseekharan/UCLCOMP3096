using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCLReadabilityMetricToolEditor
{
    class TimeTracker
    {
        IWpfTextView view;
        DateTime start;
        DateTime end;

        private List<DateTime> startTimes;
        private List<DateTime> endTimes;


        public TimeTracker(IWpfTextView view, DateTime dt)
        {
            this.view = view;
            this.start = dt;
            startTimes = new List<DateTime>();
            endTimes = new List<DateTime>();
        }

        public void PauseTimer(DateTime end)
        {
            startTimes.Add(start);
            endTimes.Add(end);

            start = new DateTime();
            end = new DateTime();
        }

        public void ResumeTimer(DateTime now)
        {
            start = now;
        }


        public List<DateTime> getStartTimes()
        {
            return startTimes;
        }

        public List<DateTime> getEndTimes()
        {
            return endTimes;
        }

        public int getNumberOfSessions()
        {
            return startTimes.Count;
        }


    }
}
