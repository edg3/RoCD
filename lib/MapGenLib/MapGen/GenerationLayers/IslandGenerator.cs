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

            var regions = v.Regions();

            Bitmap image = new Bitmap((int)width, (int)height);
            Graphics imageGraphics = Graphics.FromImage(image);

            imageGraphics.FillRectangle(Brushes.White, 0, 0, (float)width, (float)height);

            PerlinNoise pnoise = new PerlinNoise(rndm.Next());
            PerlinNoise.widthDivisor = width;
            PerlinNoise.heightDivisor = height;

            List<Polygon> waterRegions = new List<Polygon>();

            foreach (var rg in regions)
            {
                Polygon p = new Polygon(rg);
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

            image.Save(DateTime.Now.ToString("yyyyMMddhhmmss") + "_regions.bmp");
        }

    }
}
