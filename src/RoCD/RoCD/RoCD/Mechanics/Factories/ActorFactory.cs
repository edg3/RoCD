using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Mechanics.Factories
{
    public class ActorFactory
    {
        public static Actor FromCode(string code)
        {
            //TODO: Better method/layout?
            if (code == "0000") return new Actor { DrawColor = Color.Blue, TileX = 3, TileY = 2 };
            else return null;
        }
    }
}
