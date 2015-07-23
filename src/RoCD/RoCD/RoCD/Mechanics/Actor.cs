using Microsoft.Xna.Framework;
using RoCD.Helpers.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Mechanics
{
    public class Actor
    {
        //Defaults to @
        public byte TileX = 0;
        public byte TileY = 4;
        //Defaults to white
        public Color DrawColor = Color.White;

        //current
        public int X;
        public int Y;

        public virtual void Update(Map _map, Actor _player)
        {
            //default update is to roam
            _map.MoveActor(this, Map.Direction.Random);
        }

        public string Identity
        {
            get;
            set;
        }

        public Actor()
        {
            Identity = "actor";
        }
    }
}
