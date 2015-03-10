using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCLReadabilityMetricToolEditor
{
    class ExportData
    {

        private static void exportLineFrequency(String path, LineFrequency lineFrequency)
        {
            String fileName = path + "/" + "line_frequency.txt";
            using(StreamWriter tw = new StreamWriter(fileName,true))
            {
                for (int i = 0; i < lineFrequency.getNumberOfSessions()-1; i++)
                {
                    tw.WriteLine("Start session:" + lineFrequency.getStartTimes()[i]);
                    tw.WriteLine("-----");
                    for (int j = 0; j < lineFrequency.getLineCounters()[i].Count(); j++)
                    {
                        tw.WriteLine(j + 1 + "," + lineFrequency.getLineCounters()[i][j]);
                    }
                    tw.WriteLine("-----");
                    tw.WriteLine("End session:" + lineFrequency.getEndTimes()[i]);
                    tw.WriteLine("----------");
                }
                tw.Close();
            }
        }

        private static void exportEye(String path, EyeTracker eye)
        {
            String fileName = path + "/" + "eye_tracker.txt";
            using (StreamWriter tw = new StreamWriter(fileName, true))
            {
                for (int i = 0; i < eye.getNumberOfSessions() - 1; i++)
                {
                    tw.WriteLine("Start session:" + eye.getStartTimes()[i]);
                    tw.WriteLine("-----");
                    for (int j = 0; j < eye.getLineCounters()[i].Count(); j++)
                    {
                        tw.WriteLine(j + 1 + "," + eye.getLineCounters()[i][j]);
                    }
                    tw.WriteLine("-----");
                    tw.WriteLine("End session:" + eye.getEndTimes()[i]);
                    tw.WriteLine("----------");
                }
                tw.Close();
            }
        }

        private static void exportTextHighlight(String path, TextHighlightTracker textHighlight)
        {
            String fileName = path + "/" + "text_highlight.txt";
            using(StreamWriter tw = new StreamWriter(fileName,true))
            {
                for (int i = 0; i < textHighlight.getNumberOfSessions()-1; i++)
                {
                    tw.WriteLine("Start session:" + textHighlight.getStartTimes()[i]);
                    tw.WriteLine("-----");
                    for (int j = 0; j < textHighlight.getLineCounters()[i].Count(); j++)
                    {
                        tw.WriteLine(j + 1 + "," + textHighlight.getLineCounters()[i][j]);
                    }
                    tw.WriteLine("-----");
                    tw.WriteLine("End session:" + textHighlight.getEndTimes()[i]);
                    tw.WriteLine("----------");
                }
                tw.Close();
            }

        }

        private static void exportCaret(String path, CaretTracker caret)
        {
            String fileName = path + "/" + "caret_tracker.txt";
            using(StreamWriter tw = new StreamWriter(fileName,true))
            {
                for (int i = 0; i < caret.getNumberOfSessions()-1; i++)
                {
                    tw.WriteLine("Start session:" + caret.getStartTimes()[i]);
                    tw.WriteLine("-----");
                    for (int j = 0; j < caret.getLineCounters()[i].Count(); j++)
                    {
                        tw.WriteLine(j + 1 + "," + caret.getLineCounters()[i][j]);
                    }
                    tw.WriteLine("-----");
                    tw.WriteLine("End session:" + caret.getEndTimes()[i]);
                    tw.WriteLine("----------");
                }
                tw.Close();
            }
        }

        private static void exportMouse(String path, MouseTracker mouse)
        {
            String fileName = path + "/" + "mouse_tracker.txt";
            using (StreamWriter tw = new StreamWriter(fileName, true))
            {
                for (int i = 0; i < mouse.getNumberOfSessions() - 1; i++)
                {
                    tw.WriteLine("Start session:" + mouse.getStartTimes()[i]);
                    tw.WriteLine("-----");
                    for (int j = 0; j < mouse.getLineCounters()[i].Count(); j++)
                    {
                        tw.WriteLine(j + 1 + "," + mouse.getLineCounters()[i][j]);
                    }
                    tw.WriteLine("-----");
                    tw.WriteLine("End session:" + mouse.getEndTimes()[i]);
                    tw.WriteLine("----------");
                }
                tw.Close();
            }
        }

        private static void exportTime(String path, TimeTracker time)
        {
            String fileName = path + "/" + "time_tracker.txt";
            using (StreamWriter tw = new StreamWriter(fileName, true))
            {
                tw.WriteLine("startTime,endTime");
                for (int i = 0; i < time.getStartTimes().Count(); i++)
                {
                    tw.WriteLine(time.getStartTimes()[i].ToLongTimeString() + "," + time.getEndTimes()[i].ToLongTimeString());
                }
                tw.Close();
            }
        }

        public static void export(String className, DateTime now, LineFrequency lineFrequency, TextHighlightTracker textHighlight, CaretTracker caret, MouseTracker mouse, TimeTracker time, EyeTracker eye)
        {
            Debug.WriteLine("Dumping results into text file");
            //Create dateTime string to name our directory.
            String dateTime = now.ToLongDateString() + "_" + now.ToLongTimeString();
            dateTime = dateTime.Replace(":", "-");
            dateTime = dateTime.Replace("/", "--");
            //escape any characters in className
            className = className.Replace(":", "-");
            className = className.Replace("/", "--");


            //create directory 
            String path = "/" + className + "/" + dateTime;
            Directory.CreateDirectory(path);

            //now that it's created, let's start exporting data.
            exportLineFrequency(path, lineFrequency);
            exportTextHighlight(path, textHighlight);
            exportCaret(path, caret);
            exportMouse(path, mouse);
            exportTime(path, time);
            exportEye(path, eye);
        }
    }
}
