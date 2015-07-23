using RoCD.Helpers.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Mechanics.AI
{
    public interface IAIAgent
    {
        void RunAI(Actor _agent_on, Map _map, Actor _target);
    }
}
