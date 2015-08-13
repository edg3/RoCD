using MapGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenLib
{
    class Program
    {
        static void Main(string[] args)
        {
            MapGen.MapGenerator mgen = new MapGen.MapGenerator(4000, 4000);

            mgen.AddLayer(new MapGen.GenerationLayers.IslandGenerator());

            mgen.Generate();

            System.Console.ReadLine();
        }
    }
}
