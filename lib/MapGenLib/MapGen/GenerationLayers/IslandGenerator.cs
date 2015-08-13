using MapGen.Structure;
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

            //Voronoi polygons using Fortune's Algorithm https://en.wikipedia.org/wiki/Fortune%27s_algorithm
            //Implementation follows from http://blog.ivank.net/fortunes-algorithm-and-implementation.html
            //Optional: Run Lloyd relaxation 2 or 3 times https://en.wikipedia.org/wiki/Lloyd%27s_algorithm
            Voronoi v = new Voronoi();
            List<VPoint> ver = new List<VPoint>();
            List<VPoint> dir = new List<VPoint>();

            Random rndm = new Random();
            for (int i = 0; i < 500; i++)
            {
                ver.Add(new VPoint() { X = (int)(w * rndm.NextDouble()), Y = (int)(w * rndm.NextDouble()) });
                dir.Add(new VPoint() { X = rndm.NextDouble() - 0.5, Y = rndm.NextDouble() - 0.5 });
            }

            List<VEdge> edg = v.GetEdges(ver, (int)w, (int)h);

            System.Console.WriteLine("Voronoi done.");

            //check if we have all start and end edges:
            for (int i = 0; i < edg.Count; ++i)
            {
                if (null == edg[i].start)
                {
                    System.Console.WriteLine("Missing start edges...");
                    continue;
                }
                if (null == edg[i].end)
                {
                    System.Console.WriteLine("Missing end edges...");
                }
            }

            //Render voronoi:
            renderVoronoi(v, ver, dir, ground);
        }

        private void renderVoronoi(Voronoi v, List<VPoint> ver, List<VPoint> dir, uint[,] ground)
        {
            int j = 0;
            for (int i = 0; i < ver.Count; ++i)
            {
                ver[i].X += dir[j].X * width / 50;
                ver[i].Y += dir[j].Y * height / 50;

                if (ver[i].X > width) dir[j].X *= -1;
                if (ver[i].X < 0) dir[j].X *= -1;

                if (ver[i].Y > height) dir[j].Y *= -1;
                if (ver[i].Y < 0) dir[j].Y *= -1;

                ++j;
            }

            List<VEdge> edg = v.GetEdges(ver, (int)width, (int)height);

            Bitmap image = new Bitmap((int)width, (int)height);
            Graphics imageGraphics = Graphics.FromImage(image);

            imageGraphics.FillRectangle(Brushes.White, 0, 0, (float)width, (float)height);

            //Do something with these points
            for (int i = 0; i < ver.Count; ++i)
            {
                //use this for dots:
                //ver[i]
                ground[(int)(ver[i % ver.Count].X + 1000) % 1000, (int)(ver[i % ver.Count].Y + 1000) % 1000] = (uint)MapContent.Grass;
                imageGraphics.FillRectangle(Brushes.Red, (int)(ver[i % ver.Count].X + 1000) % 1000, (int)(ver[i % ver.Count].Y + 1000) % 1000, 3, 3);
            }
            //glVertex2f(-1 + 2 * (*i)->x / w - 0.01, -1 + 2 * (*i)->y / w - 0.01);
            //glVertex2f(-1 + 2 * (*i)->x / w + 0.01, -1 + 2 * (*i)->y / w - 0.01);
            //glVertex2f(-1 + 2 * (*i)->x / w + 0.01, -1 + 2 * (*i)->y / w + 0.01);
            //glVertex2f(-1 + 2 * (*i)->x / w - 0.01, -1 + 2 * (*i)->y / w + 0.01);

            //Do something with these edges
            for (int i = 0; i < edg.Count; ++i)
            {
                //use this for lines
                //edg[i]
                if ((null == edg[i].start) || (null == edg[i].end)) continue;
                imageGraphics.DrawLine(Pens.Red, new Point((int)edg[i].start.X, (int)edg[i].start.Y), new Point((int)edg[i].end.X, (int)edg[i].end.Y));
            }
            //glVertex2f(-1 + 2 * (*i)->start->x / w, -1 + 2 * (*i)->start->y / w);
            //glVertex2f(-1 + 2 * (*i)->end->x / w, -1 + 2 * (*i)->end->y / w);

            image.Save(DateTime.Now.ToString("yyyyMMddhhmmss") + ".bmp");
        }
    }
}
