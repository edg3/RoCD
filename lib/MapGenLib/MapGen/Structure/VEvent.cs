using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGen.Structure
{
    public class VEvent
    {
        public VPoint point { get; set; }
        public bool pe { get; set; }
        public double y { get; set; }
        public VParabola arch { get; set; }
        public static IComparer<VEvent> Comparer { get { return new VEventComparer(); } }

        public class VEventComparer : IComparer<VEvent>
        {
            public int Compare(VEvent x, VEvent y)
            {
                //TODO: make sure this is in the right direction
                return (int)(x.y - y.y);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p">Point at which the event occurs</param>
        /// <param name="pev">Whether is a place event or not</param>
        public VEvent(VPoint p, bool pev)
        {
            point = p;
            pe = pev;
            y = p.Y;
        }
    }
}
