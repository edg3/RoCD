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
    public class IslandGenerator : IGenerationLayer
    {
        public void Generate(uint[,] ground, uint[,] tile, uint w, uint h)
        {
            Random rndm = new Random();
            //Voronoi polygons using Fortune's Algorithm https://en.wikipedia.org/wiki/Fortune%27s_algorithm

            //1. make a bunch of dots
            List<MapPoint> _voronoi_points = new List<MapPoint>();
            
            //1.1. we want (w * h)^(1/2.25) dots {TODO:To be tweaked}
            int _num_points = (int)Math.Pow(w * h, 1 / 2.25);

            //1.2. generate the dots inside the map area, we will work with approximated 100 from each edge of the map
            for (int i = 0; i < _num_points; i++)
            {
                _voronoi_points.Add(new MapPoint()
                {
                    X = (uint)rndm.Next(100, (int)w - 100),
                    Y = (uint)rndm.Next(100, (int)h - 100)
                });
            }

            //Run Lloyd relaxation 2 or 3 times https://en.wikipedia.org/wiki/Lloyd%27s_algorithm
            
        }
    }
}
