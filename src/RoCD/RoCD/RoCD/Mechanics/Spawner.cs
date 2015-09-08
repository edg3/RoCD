using Microsoft.Xna.Framework;
using RoCD.Helpers;
using RoCD.Helpers.Tiles;
using RoCD.Mechanics.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Mechanics
{
    public class Spawner : Actor
    {
        public Spawner()
        {
            TileX = 15;
            TileY = 7;
            DrawColor = Color.Red;

            Identity = "spawner";
        }

        public string factoryCode = "0000";
        public string rareCode = "0001";

        public bool hasSpawned = false;

        public override void Update(Map _map, Actor _player)
        {
            //if (LOS to player) && (hasntyetspawned)
            if (!hasSpawned)
            {
                //check the player is in range:
                if (Math.Sqrt(Math.Pow(_player.X - X, 2) + Math.Pow(_player.Y - Y, 2)) < 16)
                {
                    //find an open neighbouring cell:
                    if (_map[X - 1, Y].Contained == null)
                    {
                        string useCode = (RoCDRndm.NextDouble() > 0.9 ? rareCode : factoryCode);
                        _map[X - 1, Y].Contained = ActorFactory.FromCode(useCode);
                        _map[X - 1, Y].Contained.X = X - 1;
                        _map[X - 1, Y].Contained.Y = Y;
                        _map.registeredActors.Add(_map[X - 1, Y].Contained);
                        hasSpawned = true;

                        CombatLog.Log("- a " + _map[X - 1, Y].Contained.Identity + " appears");
                        CombatLog.GameLog("Spawner:a " + _map[X - 1, Y].Contained.Identity + " {" + "" + "} was spawned in world at (" + (X - 1).ToString() + "," + (Y).ToString() + ")");
                    }
                }
            }
            else if (Math.Sqrt(Math.Pow(_player.X - X, 2) + Math.Pow(_player.Y - Y, 2)) > 32)
            {
                hasSpawned = false;
                CombatLog.GameLog("Spawner:spawner at (" + X.ToString() + "," + Y.ToString() + ") is now allowed to respawn");
            }
        }
    }
}
