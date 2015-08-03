using RoCD.Mechanics;
using RoCD.Mechanics.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Helpers.Tiles
{
    public class Tile
    {
        private bool pathable = true;

        public bool Pathable
        {
            get { return pathable; }
            set { pathable = value; }
        }

        public TileRenderInfo RenderInfo { get; set; }

        public Actor Contained { get; set; }

        public Inventory items = new Inventory();

    }
}
