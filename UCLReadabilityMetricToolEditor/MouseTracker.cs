﻿using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Collections;
using System.ComponentModel.Composition;
using System.Windows.Threading;

namespace UCLReadabilityMetricToolEditor
{
   
        public class MouseTracker
        {
            private IWpfTextView textView;
            //has to be static unfortunately. :( 
            private static Point mousePos;
            private static double lineNumber;

            //start/end time of each session
            private List<DateTime> startTimes;
            private List<DateTime> endTimes;

            private DateTime start;
            private DateTime end;

            private List<MouseRecord> collectedPoints = new List<MouseRecord>();

            List<List<MouseRecord>> sessions = new List<List<MouseRecord>>();

            private List<int[]> lineCounters;

            private int[] currentLineCounter;

            public MouseTracker(IWpfTextView textView, DateTime start)
            {
                this.textView = textView;
                this.start = start;
                startTimes = new List<DateTime>();
                endTimes = new List<DateTime>();
                lineCounters = new List<int[]>();
                currentLineCounter = new int[textView.TextSnapshot.LineCount];
            }

            public void timer_Tick(DateTime dt)
            {
                MouseRecord r = new MouseRecord();
                r.X = mousePos.X;
                r.Y = mousePos.Y;
                r.LineNo = lineNumber;
                currentLineCounter[(int)lineNumber]++;
                r.Time = dt;
                collectedPoints.Add(r);
                Debug.WriteLine("MOUSE: x = " + r.X + " y = " + r.Y + " line no: " + r.LineNo);
            }

            //pause timer

            public void PauseTimer(DateTime end)
            {
                //add start time to list
                startTimes.Add(start);
                endTimes.Add(end);

                sessions.Add(collectedPoints);
                lineCounters.Add(currentLineCounter);

                start = new DateTime();
                end = new DateTime();

                collectedPoints = new List<MouseRecord>();
                currentLineCounter = new int[textView.TextSnapshot.LineCount];
            }

            public void ResumeTimer(DateTime now)
            {
                start = now;
            }

            public List<List<MouseRecord>> getRecord()
            {
                return sessions;
            }

            public List<DateTime> getStartTimes()
            {
                return startTimes;
            }

            public List<DateTime> getEndTimes()
            {
                return endTimes;
            }

            public List<int[]> getLineCounters()
            {
                return lineCounters;
            }

            public int getNumberOfSessions()
            {
                return startTimes.Count;
            }

            

            [Export(typeof(IMouseProcessorProvider))]
            [Name("MouseProcessor")]
            [ContentType("code")]
            [TextViewRole(PredefinedTextViewRoles.Editable)]
            internal sealed class TestMouseProcessorProvider : IMouseProcessorProvider
            {
                public IMouseProcessor GetAssociatedProcessor(IWpfTextView view)
                {
                    return new MouseBase(view);
        
                }
            }

            internal class MouseBase : MouseProcessorBase
            {
                private IWpfTextView textView;

                public MouseBase(IWpfTextView textView)
                {
                    this.textView = textView;        
                }

                public override void PreprocessMouseMove(System.Windows.Input.MouseEventArgs e)
                {
                    base.PreprocessMouseMove(e);
                    mousePos = e.GetPosition((IInputElement)textView);

                    int startPosition = textView.TextSnapshot.GetLineNumberFromPosition(textView.TextViewLines.FirstVisibleLine.Start);
                    int endPosition = textView.TextSnapshot.GetLineNumberFromPosition(textView.TextViewLines.LastVisibleLine.Start);

                    int noOfLines = endPosition - startPosition;
                    double noOfPixels = noOfLines * textView.LineHeight;
                    double lineNumberRelativeToView = mousePos.Y / textView.LineHeight;
                    double lineNumberAbsoluteToView = lineNumberRelativeToView + (startPosition);

                    lineNumber = Math.Ceiling(lineNumberAbsoluteToView);
                    
                }

                public override void PreprocessMouseWheel(MouseWheelEventArgs e)
                {
                    base.PreprocessMouseWheel(e);
               
                    mousePos = e.GetPosition((IInputElement)textView);
                    int startPosition = textView.TextSnapshot.GetLineNumberFromPosition(textView.TextViewLines.FirstVisibleLine.Start);
                    int endPosition = textView.TextSnapshot.GetLineNumberFromPosition(textView.TextViewLines.LastVisibleLine.Start);

                    int noOfLines = endPosition - startPosition;
                    double noOfPixels = noOfLines * textView.LineHeight;
                    double lineNumberRelativeToView = mousePos.Y / textView.LineHeight;
                    double lineNumberAbsoluteToView = lineNumberRelativeToView + (startPosition);

                    lineNumber = Math.Ceiling(lineNumberAbsoluteToView);
                }
            }

           

           

        }
}
