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

        /// <summary>
        /// Function to compare 2 events by their 'y' value
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static bool Compare(VEvent l, VEvent r)
        {
            return (l.y < r.y);
        }
    }
}
