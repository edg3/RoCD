using csDelaunay;
using ImageTools.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGen.GenerationLayers
{
    //References:
    //1. http://www-cs-students.stanford.edu/~amitp/game-programming/polygon-map-generation/
    //2. https://www.reddit.com/r/roguelikedev/comments/3g67kg/sharing_saturday_62/ctvsrw6
    public class IslandGenerator : IGenerationLayer
    {
        private uint width;
        private uint height;

        public void Generate(uint[,] ground, uint[,] tile, uint w, uint h)
        {
            width = w;
            height = h;

            Random rndm = new Random();

            Rectf _bounds = new Rectf(0, 0, w, h);

            List<Vector2f> _points = new List<Vector2f>();

            int num_points = (int)Math.Max(Math.Pow(w * h, 1 / 3), 120);

            for (int i = 0; i < num_points; ++i)
            {
                _points.Add(new Vector2f(rndm.Next(100, (int)w - 100), rndm.Next(100, (int)h - 100)));
            }

            Voronoi v = new Voronoi(_points, _bounds);


            var vd = v.VoronoiDiagram();
            //v.LloydRelaxation(2); //crashes - TODO
            vd = v.VoronoiDiagram();

            Bitmap image = new Bitmap((int)width, (int)height);
            Graphics imageGraphics = Graphics.FromImage(image);

            imageGraphics.FillRectangle(Brushes.White, 0, 0, (float)width, (float)height);

            PerlinNoise pnoise = new PerlinNoise(rndm.Next());
            PerlinNoise.widthDivisor = width;
            PerlinNoise.heightDivisor = height;

            List<Polygon> waterRegions = new List<Polygon>();
            var sea_edges = new List<Edge>();

            foreach (var p in v.Polygons)
            {
                Rectf poly_bounds = p.Bounds();

                if ((PerlinNoise.TripleOctave(pnoise, new Point((int)poly_bounds.x, (int)poly_bounds.y)) < 0.475) || ((poly_bounds.x < 10) || (poly_bounds.y < 10) || (poly_bounds.x + poly_bounds.width > width - 10) || (poly_bounds.y + poly_bounds.height > height - 10)))
                {
                    waterRegions.Add(p);
                }

                for (int i = (int)Math.Floor(poly_bounds.x); i < (int)(poly_bounds.x + poly_bounds.width); i++)
                {
                    for (int j = (int)Math.Floor(poly_bounds.y); j < (int)(poly_bounds.y + poly_bounds.height); j++)
                    {
                        if (p.Contains(new Vector2f(i, j)))
                        {
                            if (waterRegions.Contains(p))
                            {
                                ground[i, j] = (uint)MapContent.Water;
                                imageGraphics.FillRectangle(Brushes.LightBlue, new Rectangle(i, j, 1, 1));
                            }
                            else
                            {
                                ground[i, j] = (uint)MapContent.Grass;
                                imageGraphics.FillRectangle(Brushes.LightGreen, new Rectangle(i, j, 1, 1));
                            }
                        }
                    }
                }
            }

            //BuildPolygonNeighbourhood(v.Polygons);

            var seaEdges = new List<Edge>();
            foreach (var edge in v.Edges)
            {
                //find edges that border land
                var pl = FindPolyForSitePoint(v.Polygons, edge.LeftSite.Coord);
                var pr = FindPolyForSitePoint(v.Polygons, edge.RightSite.Coord);

                if ((null == pl) || (null == pr)) continue; //This should never happen

                if (((waterRegions.Contains(pl)) && (!waterRegions.Contains(pr))) || ((!waterRegions.Contains(pl)) && (waterRegions.Contains(pr))))
                {
                    seaEdges.Add(edge);
                    
                    imageGraphics.DrawLine(Pens.Purple,new Point((int)edge.LeftVertex.Coord.x, (int)edge.LeftVertex.Coord.y), new Point((int)edge.RightVertex.Coord.x, (int)edge.RightVertex.Coord.y));
                }
            }

            image.Save(DateTime.Now.ToString("yyyyMMddhhmmss") + "_finalmap.bmp");
        }

        private Polygon FindPolyForSitePoint(List<Polygon> polygons, Vector2f coord)
        {
            for (int i = 0; i < polygons.Count; ++i)
            {
                if (polygons[i].Contains(coord)) return polygons[i];
            }

            return null;
        }

        class PolyNeighbour
        {
            public Polygon a;
            public Polygon b;
            public Edge e;

            public bool Has(Polygon q)
            {
                if ((a == q) || (b == q)) return true;

                return false;
            }
        }

        List<PolyNeighbour> neighborhood = new List<PolyNeighbour>();

        private void BuildPolygonNeighbourhood(List<Polygon> allPolygons)
        {
            for (int i = 0; i < allPolygons.Count; ++i)
            {
                for (int j = 0; j < allPolygons.Count; ++j)
                {
                    if (i == j) continue;
                    
                }
            }
        }

        private bool LandMassBySpace(uint[,] ground, int i, int j)
        {
            if (ground[i,j] == (uint)MapContent.Grass)
            {
                if (ground[i - 1, j] != (uint)MapContent.Grass) return true;
                if (ground[i + 1, j] != (uint)MapContent.Grass) return true;
                if (ground[i, j - 1] != (uint)MapContent.Grass) return true;
                if (ground[i, j + 1] != (uint)MapContent.Grass) return true;
            }

            return false;
        }
    }
}
