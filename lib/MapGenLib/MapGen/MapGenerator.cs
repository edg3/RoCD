using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGen
{
    public class MapGenerator
    {
        private uint width;

        public uint Width
        {
            get { return width; }
            private set { width = value; }
        }

        private uint height;

        public uint Height
        {
            get { return height; }
            private set { height = value; }
        }

        uint[,] _ground;
        uint[,] _tile;
        public MapGenerator(uint w, uint h)
        {
            Width = w;
            Height = h;

            _ground = new uint[w, h];
            _tile = new uint[w, h];

            System.Console.WriteLine("Ready to accept generating layers. Map size " + width.ToString() + "x" + height.ToString());
        }

        private List<IGenerationLayer> _layers = new List<IGenerationLayer>();

        public void AddLayer(IGenerationLayer generationLayer)
        {
            _layers.Add(generationLayer);
            System.Console.WriteLine("Accepted layer " + generationLayer.ToString());
        }

        public void Generate()
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                System.Console.WriteLine(_layers[i].ToString() + " running Generate.");
                _layers[i].Generate(_ground, _tile, Width, Height);
            }

            System.Console.WriteLine("Generation complete");
        }
    }
}
