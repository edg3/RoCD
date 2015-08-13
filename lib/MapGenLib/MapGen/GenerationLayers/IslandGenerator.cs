using MapGen.Structure;
using System;
using System.Collections.Generic;
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
            //Run Lloyd relaxation 2 or 3 times https://en.wikipedia.org/wiki/Lloyd%27s_algorithm
            Voronoi v = new Voronoi();
            List<VPoint> ver = new List<VPoint>();
            List<VPoint> dir = new List<VPoint>();

            Random rndm = new Random();
            for (int i = 0; i < 50; i++)
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
            renderVoronoi(v, ver, dir);
        }

        private void renderVoronoi(Voronoi v, List<VPoint> ver, List<VPoint> dir)
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

            //Do something with these points
            for (int i = 0; i < ver.Count; ++i)
            {
                //use this for dots:
                //ver[i]
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
            }
            //glVertex2f(-1 + 2 * (*i)->start->x / w, -1 + 2 * (*i)->start->y / w);
            //glVertex2f(-1 + 2 * (*i)->end->x / w, -1 + 2 * (*i)->end->y / w);
        }
    }
}
