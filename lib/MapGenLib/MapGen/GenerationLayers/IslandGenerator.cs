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
        public void Generate(uint[,] ground, uint[,] tile, uint w, uint h)
        {
            //Voronoi polygons using Fortune's Algorithm https://en.wikipedia.org/wiki/Fortune%27s_algorithm
            //Implementation follows from http://blog.ivank.net/fortunes-algorithm-and-implementation.html
            //Run Lloyd relaxation 2 or 3 times https://en.wikipedia.org/wiki/Lloyd%27s_algorithm
            
        }
    }
}
