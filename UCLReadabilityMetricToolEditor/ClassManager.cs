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

    /// <summary>
    /// Manages the analytics for a C# document.
    /// </summary>
    public class ClassManager
    {

        private String className = "";
        private IWpfTextView textView;
        private DispatcherTimer timer;

        /// Tracking objects
        private LineFrequency lineFrequency;
        private MouseTracker mouseTracker;
        private CaretTracker caretTracker;
        private TimeTracker timeTracker;
        private TextHighlightTracker textHighlightTracker;
        private EyeTracker eyeTracker; 

        public ClassManager(String className, IWpfTextView textView)
        {
            //get coordinate of corners of editor.
            this.className = className;
            
            this.textView = textView;
            DateTime dt = DateTime.Now;
            configureTimer();
            lineFrequency = new LineFrequency(textView, dt);
            mouseTracker = new MouseTracker(textView,dt);
            caretTracker = new CaretTracker(textView, dt);
            timeTracker = new TimeTracker(textView, dt);
            eyeTracker = new EyeTracker(textView, dt);
            textHighlightTracker = new TextHighlightTracker(textView, dt);
            SubscribeToListeners();
        }

        private void configureTimer()
        {
            
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1.0);
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.UtcNow;
            lineFrequency.timer_Tick(sender, e);
            mouseTracker.timer_Tick(currentTime);
            caretTracker.timer_Tick(sender, e);
            textHighlightTracker.timer_Tick(sender, e);
            eyeTracker.timer_Tick(sender, e);
            
        }

        public IWpfTextView GetTextView()
        {
            return textView;
        }

        public String GetClassName()
        {
            return className;
        }

        private void SubscribeToListeners()
        {
            textView.GotAggregateFocus += textView_GotAggregateFocus;
            textView.LostAggregateFocus += textView_LostAggregateFocus;
        }

        /// <summary>
        /// Document has been closed, so stop tracking.
        /// </summary>
        /// 

        public void Close()
        {
            DateTime now = DateTime.Now;
            lineFrequency.PauseTimer(now);
            mouseTracker.PauseTimer(now);
            caretTracker.PauseTimer(now);
            eyeTracker.PauseTimer(now);
            timeTracker.PauseTimer(now);
            textHighlightTracker.PauseTimer(now);
          
            Debug.WriteLine("Dumping results into text file");
            //create directory to store results.
            ExportData.export(className, now, lineFrequency, textHighlightTracker, caretTracker, mouseTracker, timeTracker, eyeTracker);

               
        }

        /// <summary>
        /// When focus on the document is lost, this event is triggered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textView_LostAggregateFocus(object sender, EventArgs e)
        {
            //pause tracking.
            DateTime now = DateTime.Now;
            Debug.WriteLine(className + " lost focus!");
            lineFrequency.PauseTimer(now);
            mouseTracker.PauseTimer(now);
            caretTracker.PauseTimer(now);
            timeTracker.PauseTimer(now);
            textHighlightTracker.PauseTimer(now);
            eyeTracker.PauseTimer(now);
            timer.Stop();
        }

        /// <summary>
        /// When focus on the document is obtained, this event is triggered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textView_GotAggregateFocus(object sender, EventArgs e)
        {
            //resume tracking.
            DateTime now = DateTime.Now;
            Debug.WriteLine(className + " got focus!");
            lineFrequency.ResumeTimer(now);
            mouseTracker.ResumeTimer(now);
            caretTracker.ResumeTimer(now);
            timeTracker.ResumeTimer(now);
            textHighlightTracker.ResumeTimer(now);
            eyeTracker.ResumeTimer(now);
            timer.Start();
        }
        
    }
}
