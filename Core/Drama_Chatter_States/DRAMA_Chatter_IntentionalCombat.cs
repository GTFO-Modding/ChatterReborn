using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Drama_Chatter_States
{
    public class DRAMA_Chatter_IntentionalCombat : DRAMA_Chatter_Combat
    {
        protected override bool StartCombatDialogue => true;
    }
}
