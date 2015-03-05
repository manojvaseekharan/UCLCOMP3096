using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace UCLReadabilityMetricToolEditor
{
    public class LineFrequency
    {
        private IWpfTextView view;
        private DateTime start;
        private DateTime end;

        /// <summary>
        /// The interval between samples (in milliseconds).
        /// </summary>
        private const int interval = 1000;

        private DispatcherTimer timer;

        //stores the start/end time of each session.
        private List<DateTime> startTimes;
        private List<DateTime> endTimes;

        //stores number of lines looked at in each session.
        private List<int[]> lineCounters;

        //current session's line counter
        private int[] currentLineCounter;

        public LineFrequency(IWpfTextView view, DateTime dt)
        {
            this.view = view;
            this.start = dt;
            startTimes = new List<DateTime>();
            endTimes = new List<DateTime>();
            lineCounters = new List<int[]>();
            currentLineCounter = new int[view.TextSnapshot.LineCount];
        }

        public List<int[]> getLineCounters()
        {
            return lineCounters;
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


        public void PauseTimer(DateTime end)
        {
            Debug.WriteLine("Timer paused. Laogging details into list");

            //add them to list.
            startTimes.Add(start);
            endTimes.Add(end);
            lineCounters.Add(currentLineCounter);

            //clear variables
            start = new DateTime();
            end = new DateTime();

            //reinitialise the linecounter
            currentLineCounter = new int[view.TextSnapshot.LineCount];
        }

        public void ResumeTimer(DateTime now)
        {
            Debug.WriteLine("Timer resumed.");
            start = now;
        }


        /// <summary>
        /// Every second, get the first and last index of lines on screen, increment lines accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void timer_Tick(object sender, EventArgs e)
        {
            int firstVisibleLine = view.TextSnapshot.GetLineNumberFromPosition(view.TextViewLines.FirstVisibleLine.Start);
            int lastVisibleLine = view.TextSnapshot.GetLineNumberFromPosition(view.TextViewLines.LastVisibleLine.Start);
            for (int i = firstVisibleLine; i <= lastVisibleLine; i++)
            {
                currentLineCounter[i]++;
            }
        }

        public void setIWpfTextView(IWpfTextView textView)
        {
            view = textView;
        }

        public void startTimer()
        {
            timer.Start();
        }

        public void setStartDateTime(DateTime start)
        {
            this.start = start;
        }


        public void setEndDateTime(DateTime end)
        {
            this.end = end;
        }

        public void stopTimer()
        {
            timer.Stop();
        }

      
    }
}
