using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGen.Structure
{
    public class Voronoi
    {
        List<VPoint> places = new List<VPoint>();
        List<VEdge> edges = null;
        double width;
        double height;
        VParabola root;
        double ly;

        List<VEvent> deleted = new List<VEvent>();
        List<VPoint> points = new List<VPoint>();

        List<VEvent> queue = new List<VEvent>();

        public Voronoi()
        {

        }

        public List<VEdge> GetEdges(List<VPoint> v, int w, int h)
        {
            places = v;
            width = w;
            height = h;
            root = null;

            if (null == edges) edges = new List<VEdge>();
            else
            {
                points.Clear();
                edges.Clear();
                queue.Clear();
            }

            for (int i = 0; i < places.Count; ++i)
            {
                queue.Add(new VEvent(places[i], true));
            }

            queue.Sort(VEvent.Comparer);

            VEvent e;
            while (queue.Count > 0)
            {
                e = queue[0]; //reversed order to test
                queue.Remove(e);

                ly = e.point.Y;
                if (deleted.Contains(e) && (deleted.LastOrDefault() != e))
                {
                    deleted.Remove(e); continue;
                }
                if (e.pe) InsertParabola(e.point);
                else RemoveParabola(e);
            }

            FinishEdge(root);
            root = null;

            for (int i = 0; i < edges.Count; ++i)
            {
                if (null != edges[i].neighbour)
                {
                    edges[i].start = edges[i].neighbour.end;
                    edges[i].neighbour = null; //dereference
                }
            }

            return edges;
        }

        void InsertParabola(VPoint p)
        {
            if (null == root)
            {
                root = new VParabola(p);
                return;
            }

            if (root.isLeaf && (root.site.Y - p.Y < 1))
            {
                VPoint fp = root.site;
                root.isLeaf = false;
                root.SetLeft(new VParabola(fp));
                root.SetRight(new VParabola(p));
                VPoint s = new VPoint() { X = (p.X + fp.X) / 2, Y = (int)height };
                points.Add(s);
                if (p.X > fp.X) root.edge = new VEdge(s, fp, p);
                else root.edge = new VEdge(s, p, fp);
                edges.Add(root.edge);
                return;
            }

            VParabola par = GetParabolaByX(p.X);

            if (null != par.cEvent)
            {
                deleted.Add(par.cEvent);
                par.cEvent = null;
            }

            VPoint start = new VPoint() { X = p.X, Y = (int)GetY(par.site, p.X) };
            points.Add(start);

            //if ((null == start) || (null == par.site) || (null == p)) return;

            VEdge el = new VEdge(start, par.site, p);
            VEdge er = new VEdge(start, p, par.site);

            el.neighbour = er;
            edges.Add(el);

            par.edge = er;
            par.isLeaf = false;

            VParabola p0 = new VParabola(par.site);
            VParabola p1 = new VParabola(p);
            VParabola p2 = new VParabola(par.site);

            par.SetRight(p2);
            par.SetLeft(new VParabola());
            par.Left().edge = el;

            par.Left().SetLeft(p0);
            par.Left().SetRight(p1);

            CheckCircle(p0);
            CheckCircle(p2);
        }

        void RemoveParabola(VEvent e)
        {
            VParabola p1 = e.arch;

            VParabola xl = VParabola.GetLeftParent(p1);
            VParabola xr = VParabola.GetRightParent(p1);

            VParabola p0 = VParabola.GetLeftChild(xl);
            VParabola p2 = VParabola.GetRightChild(xr);

            if (p0 == p2) System.Console.WriteLine("error - right and left parabola has the same focus!");

            if (null != p0.cEvent)
            {
                deleted.Add(p0.cEvent);
                p0.cEvent = null;
            }
            if (null != p2.cEvent)
            {
                deleted.Add(p2.cEvent);
                p2.cEvent = null;
            }

            VPoint p = new VPoint() { X = e.point.X, Y = (int)GetY(p1.site, e.point.X) };
            points.Add(p);

            xl.edge.end = p;
            xr.edge.end = p;

            VParabola higher = null;
            VParabola par = p1;
            while (par != root)
            {
                par = par.parent;
                if (par == xl) higher = xl;
                if (par == xr) higher = xr;
            }
            higher.edge = new VEdge(p, p0.site, p2.site);
            edges.Add(higher.edge);

            VParabola gparent = p1.parent.parent;
            if (p1.parent.Left() == p1)
            {
                if (gparent.Left() == p1.parent) gparent.SetLeft(p1.parent.Right());
                if (gparent.Right() == p1.parent) gparent.SetRight(p1.parent.Right());
            }
            else
            {
                if (gparent.Left() == p1.parent) gparent.SetLeft(p1.parent.Left());
                if (gparent.Right() == p1.parent) gparent.SetRight(p1.parent.Left());
            }

            CheckCircle(p0);
            CheckCircle(p2);
        }

        void FinishEdge(VParabola n)
        {
            if (n.isLeaf) { return; }
            double mx;
            //if (null == n.edge) return;
            if (n.edge.direction.X > 0) mx = Math.Max(width, n.edge.start.X + 10);
            else mx = Math.Min(0, n.edge.start.X - 10);

            VPoint end = new VPoint() { X = (int)mx, Y = (int)(mx * n.edge.f + n.edge.g) };
            n.edge.end = end;
            points.Add(end);

            FinishEdge(n.Left());
            n.SetLeft(null);
            FinishEdge(n.Right());
            n.SetRight(null);
        }

        double GetXOfEdge(VParabola par, double y)
        {
            VParabola left = VParabola.GetLeftChild(par);
            VParabola right = VParabola.GetRightChild(par);

            //if ((null == left) || (null == right)) return 0.0;

            VPoint p = left.site;
            VPoint r = right.site;

            double dp = 2.0 * (p.Y - y);
            double a1 = 1.0 / dp;
            double b1 = -2.0 * p.X / dp;
            double c1 = y + dp / 4 + p.X * p.X / dp;

            dp = 2.0 * (r.Y - y);
            double a2 = 1.0 / dp;
            double b2 = -2.0 * r.X / dp;
            double c2 = ly + dp / 4 + r.X * r.X / dp;

            double a = a1 - a2;
            double b = b1 - b2;
            double c = c1 - c2;

            double disc = b * b - 4 * a * c;
            double x1 = (-b + Math.Sqrt(disc)) / (2 * a);
            double x2 = (-b - Math.Sqrt(disc)) / (2 * a);

            double ry;
            if (p.Y < r.Y) ry = Math.Max(x1, x2);
            else ry = Math.Min(x1, x2);

            return ry;
        }

        VParabola GetParabolaByX(double xx)
        {
            VParabola par = root;
            double x = 0.0;

            while (!par.isLeaf)
            {
                x = GetXOfEdge(par, ly);
                if (x > xx) par = par.Left();
                else par = par.Right();
            }
            return par;
        }

        double GetY(VPoint p, double x)
        {
            double dp = 2 * (p.X - ly);
            double a1 = 1 / dp;
            double b1 = -2 * p.X / dp;
            double c1 = ly + dp / 4 + p.X * p.X / dp;

            return (a1 * x * x + b1 * x + c1);
        }

        void CheckCircle(VParabola b)
        {
            VParabola lp = VParabola.GetLeftParent(b);
            VParabola rp = VParabola.GetRightParent(b);

            VParabola a = VParabola.GetLeftChild(lp);
            VParabola c = VParabola.GetRightChild(rp);

            if ((null == a) || (null == c) || (a.site == c.site)) return;

            VPoint s = null;
            s = GetEdgeIntersection(lp.edge, rp.edge);
            if (null == s) return;

            double dx = a.site.X - s.X;
            double dy = a.site.Y - s.Y;

            double d = Math.Sqrt((dx * dx) + (dy * dy));

            if (s.Y - d >= ly) return;

            VEvent e = new VEvent(new VPoint() { X = s.X, Y = (int)(s.Y - d) }, false);
            points.Add(e.point);
            b.cEvent = e;
            e.arch = b;
            queue.Add(e);
        }

        VPoint GetEdgeIntersection(VEdge a, VEdge b)
        {
            double x = (b.g - a.g) / (a.f - b.f);
            double y = (a.f * x) + a.g;

            if ((x - a.start.X) / a.direction.X < 0) return null;
            if ((y - a.start.Y) / a.direction.Y < 0) return null;

            if ((x - b.start.X) / b.direction.X < 0) return null;
            if ((y - b.start.Y) / b.direction.Y < 0) return null;

            VPoint p = new VPoint() { X = (int)x, Y = (int)y };
            points.Add(p);
            return p;
        }
    }
}
