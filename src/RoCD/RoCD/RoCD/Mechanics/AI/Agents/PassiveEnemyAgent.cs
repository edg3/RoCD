using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Mechanics.AI.Agents
{
    public class PassiveEnemyAgent : IAIAgent
    {
        public const int ChanceToRoam = 5; //Out of 100

        public bool agressive = false;

        public void RunAI(Actor _agent_on, Helpers.Tiles.Map _map, Actor _target)
        {
            Random rndm = new Random();
            if (agressive)
            {
                _map.MoveActor(_agent_on, _map.AStarTo(_agent_on, _target));
                if (Math.Sqrt(Math.Pow(_agent_on.X - _target.X, 2) + Math.Pow(_agent_on.Y - _target.Y, 2)) < 1.5)
                {
                    //Attack Player!
                }
            }
            else
            {
                //Chance to roam
                if (rndm.Next(100) < ChanceToRoam)
                {
                    _map.MoveActor(_agent_on, Helpers.Tiles.Map.Direction.Random);
                }
            }
        }
    }
}
