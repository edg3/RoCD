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
        public string factoryCode = "0000";

        public bool hasSpawned = false;

        public override void Update(Map _map)
        {
            //if (LOS to player) && (hasntyetspawned)
            if (!hasSpawned)
            {
                //find an open neighbouring cell:
                if (_map[X - 1, Y].Contained == null)
                {
                    _map[X - 1, Y].Contained = ActorFactory.FromCode(factoryCode);
                    _map[X - 1, Y].Contained.X = X - 1;
                    _map[X - 1, Y].Contained.Y = Y;
                    _map.registeredActors.Add(_map[X - 1, Y].Contained);
                    hasSpawned = true;
                }
            }
        }
    }
}
