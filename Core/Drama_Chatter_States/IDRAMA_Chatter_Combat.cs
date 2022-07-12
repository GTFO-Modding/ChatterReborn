using ChatterReborn.Machines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Drama_Chatter_States
{
    public interface IDRAMA_Chatter_Combat
    {
        void SetupCombatState(CombatStateMachine combatMachine);
    }
}
