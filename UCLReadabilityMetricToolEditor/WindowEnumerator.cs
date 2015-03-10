using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace UCLReadabilityMetricToolEditor
{
    class WindowEnumerator
    {
        static int heightWindow = 0;
        static int widthWindow = 0;

        static RECT rectangleOfWindow;

        #region Externs and Associated Things
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // Callback Declaration
        public delegate bool EnumWindowsCallback(IntPtr hwnd, int lParam);
        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWindowsCallback callPtr, int lParam);

        public static bool ReportWindow(IntPtr hwnd, int lParam)
        {
            uint processId = 0;
            uint threadId = GetWindowThreadProcessId(hwnd, out processId);

            RECT rt = new RECT();
            bool locationLookupSucceeded = GetWindowRect(hwnd, out rt);

            if (locationLookupSucceeded)
            {

            }
            else
            {
                Debug.WriteLine("  Unable to lookup position through GetWindowRect()");
            }

            try
            {
                Process ownerProcess = Process.GetProcessById((int)processId);
                bool isMainWindow = ownerProcess.MainWindowHandle.Equals(hwnd);
                String name = ownerProcess.ProcessName;
                if (name.Equals("devenv") && rt.Bottom != 0 && rt.Top != 0 && (rt.Bottom-rt.Top == heightWindow+17))
                {
                    Debug.WriteLine("left = " + rt.Left + ", top = " + rt.Top + ", right = " + rt.Right + ", bottom = " + rt.Bottom);
                    rectangleOfWindow = rt;
                }
                  
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("  Unable to lookup Process Information for pid {0}", processId));
                Debug.WriteLine(string.Format("    - Exception of type {0} occurred.", ex.GetType().Name));
            }

            return true;
        }
        #endregion//Externs and Associated Things

        public static WindowRectangle CheckWindow(int height, int width)
        {

            // Have to declare a delegate so that a thunk is created, so that win32 may call us back.
            EnumWindowsCallback callBackFn = new EnumWindowsCallback(ReportWindow);
            heightWindow = height;
            widthWindow = width;
            EnumWindows(callBackFn, 0);
            WindowRectangle w = new WindowRectangle();
            w.Left = rectangleOfWindow.Left;
            w.Right = rectangleOfWindow.Right;
            w.Top = rectangleOfWindow.Top;
            w.Bottom = rectangleOfWindow.Bottom;
            return w;

        }
    }
}
