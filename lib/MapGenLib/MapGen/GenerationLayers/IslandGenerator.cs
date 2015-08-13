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

            for (int i = 0; i < 500; ++i)
            {
                _points.Add(new Vector2f(rndm.Next(100, (int)w - 100), rndm.Next(100, (int)h - 100)));
            }

            Voronoi v = new Voronoi(_points, _bounds);

            //v.LloydRelaxation(2);
            var vd = v.VoronoiDiagram();

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

            image.Save(DateTime.Now.ToString("yyyyMMddhhmmss") + ".bmp");
        }

    }
}
