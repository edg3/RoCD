using Microsoft.Xna.Framework;
using RoCD.Helpers.Tiles;
using RoCD.Mechanics.AI;
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

        public IAIAgent AIAgent;

        public virtual void Update(Map _map, Actor _player)
        {
            if (null != AIAgent) AIAgent.RunAI(this, _map, _player);
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
