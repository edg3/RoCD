using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGen.Structure
{
    public class VEdge
    {
        public VPoint start { get; set; }
        public VPoint end { get; set; }
        public VPoint direction { get; set; }
        public VPoint left { get; set; }
        public VPoint right { get; set; }

        public double f;
        public double g;

        public VEdge neighbour { get; set; }

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="s">Starting point</param>
        /// <param name="a">Left point</param>
        /// <param name="b">Right point</param>
        public VEdge(VPoint s, VPoint a, VPoint b)
        {
            
            start = s;
            left = a;
            right = b;

            f = (b.X - a.X) / (a.Y - b.Y);
            g = s.Y - f * s.X;
            direction = new VPoint()
            {
                Y = b.Y - a.Y,
                X = -(b.X - a.X)
            };
        }
    }
}
