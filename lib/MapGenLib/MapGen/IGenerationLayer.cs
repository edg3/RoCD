using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGen
{
    public interface IGenerationLayer
    {
        void Generate(uint[,] ground, uint[,] tile, uint w, uint h);
    }
}
