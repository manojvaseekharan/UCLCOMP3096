﻿using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCLReadabilityMetricToolEditor
{
    class CaretTracker
    {
        IWpfTextView view;
        DateTime start;
        DateTime end;

        private List<DateTime> startTimes;
        private List<DateTime> endTimes;

        private List<int[]> lineCounters;

        private int[] currentLineCounter;


        public CaretTracker(IWpfTextView view, DateTime dt)
        {
            this.view = view;
            this.start = dt;
            startTimes = new List<DateTime>();
            endTimes = new List<DateTime>();
            lineCounters = new List<int[]>();
            currentLineCounter = new int[view.TextSnapshot.LineCount];
        }

        public void PauseTimer(DateTime end)
        {
            startTimes.Add(start);
            endTimes.Add(end);
            lineCounters.Add(currentLineCounter);

            start = new DateTime();
            end = new DateTime();

            currentLineCounter = new int[view.TextSnapshot.LineCount];
        }

        public void ResumeTimer(DateTime now)
        {
            start = now;
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            int textCaretLine = view.TextSnapshot.GetLineNumberFromPosition(view.Caret.ContainingTextViewLine.Start);
            currentLineCounter[textCaretLine]++;
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
    }
}
