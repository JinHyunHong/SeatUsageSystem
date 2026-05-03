using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatUsageSystem.UI.Layouts
{
    public readonly struct SeatPosition
    {
        public double X { get; }
        public double Y { get; }

        public SeatPosition(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
