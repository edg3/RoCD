using csDelaunay;
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

            int num_points = (int)Math.Max(Math.Pow(w * h, 1 / 2.66), 350);

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

            foreach (var segmnt in vd)
            {
                try {
                    imageGraphics.DrawLine(Pens.Red, new Point((int)segmnt.p0.x, (int)segmnt.p0.y), new Point((int)segmnt.p1.x, (int)segmnt.p1.y));
                }
                catch { }
            }

            image.Save(DateTime.Now.ToString("yyyyMMddhhmmss") + "_lines.bmp");

            foreach (var rg in regions)
            {
                Polygon p = new Polygon(rg);
                Rectf poly_bounds = p.Bounds();

                Brush myBrush = RandomBrush();

                for (int i = (int)poly_bounds.x; i < (int)(poly_bounds.x + poly_bounds.width); i++)
                {
                    for (int j = (int)poly_bounds.y; j < (int)(poly_bounds.y + poly_bounds.height); j++)
                    {
                        try
                        {
                            if (p.Contains(new Vector2f(i, j)))
                                imageGraphics.FillRectangle(myBrush, new Rectangle(i, j, 1, 1));
                        } catch { }
                    }
                }
            }

            image.Save(DateTime.Now.ToString("yyyyMMddhhmmss") + "_regions.bmp");
        }

        private Brush RandomBrush()
        {
            Random rndm = new Random();
            int choice = rndm.Next(5);

            switch (choice)
            {
                case 0: return Brushes.Blue;
                case 1: return Brushes.Green;
                case 2: return Brushes.Orange;
                case 3: return Brushes.Yellow;
                case 4: return Brushes.Violet;
            }

            return Brushes.Red;
        }

    }
}
