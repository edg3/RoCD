using RoCD.Helpers;
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
            if (agressive)
            {
                _map.MoveActor(_agent_on, _map.AStarTo(_agent_on, _target));
                if (Math.Sqrt(Math.Pow(_agent_on.X - _target.X, 2) + Math.Pow(_agent_on.Y - _target.Y, 2)) < 1.5)
                {
                    CombatLog.GameLog("PassiveEnemyAgent: agressive, close enough to attack player.");
                    //Attack target!
                    (_agent_on as Creature).meleeAttack(_target as Creature);
                }
            }
            else
            {
                //check distance, if too far remove:
                if (Math.Sqrt(Math.Pow(_agent_on.X - _target.X, 2) + Math.Pow(_agent_on.Y - _target.Y, 2)) > 50)
                {
                    CombatLog.GameLog("PassiveEnemyAgent: decided too far from player, time to die.");
                    (_agent_on as Creature).set(Creature.CURRHP, 0);
                    return;
                }

                //Chance to roam
                if (RoCDRndm.Next(100) < ChanceToRoam)
                {
                    CombatLog.GameLog("PassiveEnemyAgent: I will roam.");
                    _map.MoveActor(_agent_on, Helpers.Tiles.Map.Direction.Random);
                }
            }
        }
    }
}
