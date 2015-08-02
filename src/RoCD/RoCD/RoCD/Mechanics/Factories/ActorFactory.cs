using Microsoft.Xna.Framework;
using RoCD.Helpers;
using RoCD.Mechanics.AI.Agents;
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
            CombatLog.GameLog("ActorFactory:inflating fromCode (" + code + ")");
            //TODO: Better method/layout?
            if (code == "0000") return new Creature { DrawColor = Color.White, TileX = 3, TileY = 7, Identity = "slime", AIAgent = new PassiveEnemyAgent() };
            else if (code == "0001") return new Creature { DrawColor = Color.Red, TileX = 3, TileY = 7, Identity = "super slime", AIAgent = new PassiveEnemyAgent() };
            else return null;
        }
    }
}
