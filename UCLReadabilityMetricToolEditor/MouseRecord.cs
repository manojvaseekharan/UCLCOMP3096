using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCLReadabilityMetricToolEditor
{
    public class MouseRecord
    {
        
            private double x;
            private double y;
            private double lineNo;

            private DateTime time;

            public double X
            {
                get { return x; }
                set { x = value; }
            }

            public double Y
            {
                get { return y; }
                set { y = value; }
            }


            public DateTime Time
            {
                get { return time; }
                set { time = value; }
            }

            public double LineNo
            {
                get { return lineNo; }
                set { lineNo = value; }
            }

        
    }
}
