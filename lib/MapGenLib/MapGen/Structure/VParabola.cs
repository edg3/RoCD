using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGen.Structure
{
    public class VParabola
    {
        public bool isLeaf { get; set; }
        public VPoint site { get; set; }
        public VEdge edge { get; set; }
        public VEvent cEvent { get; set; }
        public VParabola parent;

        private VParabola _left { get; set; }
        private VParabola _right { get; set; }

        public void SetLeft(VParabola p) { _left = p; p.parent = this; }
        public void SetRight(VParabola p) { _right = p;  p.parent = this; }
        public VParabola Left() { return _left; }
        public VParabola Right() { return _right; }

        public VParabola()
        {
            isLeaf = false;
        }

        public VParabola(VPoint s)
        {
            site = s;
            isLeaf = true;
        }

        public static VParabola GetLeft(VParabola p)
        {
            return GetLeftChild(GetLeftParent(p));
        }

        public static VParabola GetRight(VParabola p)
        {
            return GetRightChild(GetRightParent(p));
        }

        public static VParabola GetLeftParent(VParabola p)
        {
            VParabola par = p.parent;
            VParabola pLast = p;
            while (par.Left() == pLast)
            {
                if (null == par.parent) return null;
                pLast = par;
                par = par.parent;
            }
            return par;
        }

        public static VParabola GetRightParent(VParabola p)
        {
            VParabola par = p.parent;
            VParabola pLast = p;
            while (par.Right() == pLast)
            {
                if (null == par.parent) return null;
                pLast = par;
                par = par.parent;
            }
            return par;
        }

        public static VParabola GetLeftChild(VParabola p)
        {
            if (null == p) return null;
            VParabola par = p.Left();
            while (!par.isLeaf) par = par.Right();
            return par;
        }

        public static VParabola GetRightChild(VParabola p)
        {
            if (null == p) return null;
            VParabola par = p.Right();
            while (!par.isLeaf) par = par.Left();
            return par;
        }
    }
}
