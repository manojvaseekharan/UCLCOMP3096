using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UCLReadabilityMetricToolEditor
{
    class GazeRecord
    {
        public Point point;
        public int line;

        public GazeRecord(Point pt, int eline)
        {
            point = pt;
            line = eline;
        }



    }
}
