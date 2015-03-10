using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TETCSharpClient;
using TETCSharpClient.Data;
using System.Diagnostics;
using System.Windows;
using TETControls.Calibration;
using MessageBox = System.Windows.MessageBox;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace UCLReadabilityMetricToolEditor
{
    public class EyeTracker : IGazeListener
    {
        private Point eyePoint;
        private IWpfTextView textView;

        //degree of rounding.
        private static int GAZE_ROUND = 1;

        //line number currently being looked at by user's eyes.
        private double lineNumber = 0;

        private List<DateTime> startTimes;
        private List<DateTime> endTimes;

        private DateTime start;
        private DateTime end;

        private List<int[]> lineCounters;

        private int[] currentLineCounter;

        private WindowRectangle wr;

        public EyeTracker(IWpfTextView textView, DateTime dt)
        {
            ///<summary>
            ///connect to TET server. note: SERVER MUST BE RUNNING AND TET PRE-CALIBRATED
            ///Also, check trackbox before starting simulation.
            ///Aim for a 5 star calibration with 16 points.
            ///</summary>

            GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0, GazeManager.ClientMode.Push);
            GazeManager.Instance.AddGazeListener(this);
            this.textView = textView;
            startTimes = new List<DateTime>();
            this.start = dt;
            endTimes = new List<DateTime>();
            lineCounters = new List<int[]>();
            currentLineCounter = new int[textView.TextSnapshot.LineCount];

        }

        public void OnGazeUpdate(GazeData gazeData)
        {
            //Get Gaze data
            double gX = gazeData.RawCoordinates.X;
            double gY = gazeData.RawCoordinates.Y;

            //Smoothen data for more meaningful results.
            double eX = roundNumber(gX, GAZE_ROUND);
            double eY = roundNumber(gY, GAZE_ROUND);

            //prevent negative values
            if (eX < 0)
            {
                eX = 0;
            }
                
            if (eY < 0)
            {
                eY = 0;
            }
               
            eyePoint.X = eX;
            eyePoint.Y = eY;

        }

        private double roundNumber(double num, int roundby)
        {
            return (Math.Round(num / roundby) * roundby);

        }

        public void timer_Tick(object sender, EventArgs e)
        {
            //get coordinates of textview
            if(wr.Equals(null) || wr.Left == 0)
            {
                wr = WindowEnumerator.CheckWindow((int)textView.ViewportHeight, (int)textView.ViewportWidth);
                return;
            }
            
            if(eyePoint.X < wr.Left || eyePoint.Y < wr.Top || eyePoint.Y > wr.Bottom || eyePoint.X > wr.Right)
            {
                //outside of textview, so return, no update.
                return;
            }
            else
            {
                int startPosition = textView.TextSnapshot.GetLineNumberFromPosition(textView.TextViewLines.FirstVisibleLine.Start);
                int endPosition = textView.TextSnapshot.GetLineNumberFromPosition(textView.TextViewLines.LastVisibleLine.Start);

                int noOfLines = endPosition - startPosition;
                double noOfPixels = noOfLines * textView.LineHeight;
                double lineNumberRelativeToView = (eyePoint.Y-wr.Top) / textView.LineHeight;
                double lineNumberAbsoluteToView = lineNumberRelativeToView + (startPosition);

                lineNumber = Math.Ceiling(lineNumberAbsoluteToView);

                currentLineCounter[(int)lineNumber - 1]++;
            }
        }

        public void PauseTimer(DateTime end)
        {
            //add start time to list
            //add them to list.
            startTimes.Add(start);
            endTimes.Add(end);
            lineCounters.Add(currentLineCounter);

            //clear variables
            start = new DateTime();
            end = new DateTime();

            //reinitialise the linecounter
            currentLineCounter = new int[textView.TextSnapshot.LineCount];
        }

        public void ResumeTimer(DateTime now)
        {
            start = now;
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
