using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGen
{
    public enum MapContent
    {
        /// <summary>
        /// Major features
        /// </summary>
        Mountain = 0,
        Water,
        Forest,
        Grass,
        Dirt,
        Path,
        Inside,
        Wall

        /// <summary>
        /// Minor features
        /// </summary>
    }

    public enum TileContent
    {
        /// <summary>
        /// Major features
        /// </summary>
        Boulder
    }
}
