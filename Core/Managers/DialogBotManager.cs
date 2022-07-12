using ChatterReborn.Attributes;
using ChatterReborn.Extra;
using ChatterReborn.Drama_Chatter_States;
using ChatterReborn.Utils;
using GameData;
using Player;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Managers
{
    public class DialogBotManager : ChatterManager<DialogBotManager>
    {
        public static void OnHitReactBot(PlayerAgent playerAgent)
        {
            var state = DramaChatterManager.GetMachine(playerAgent).CurrentState;
            var combat_state = state as DRAMA_Chatter_Combat;
            if (combat_state != null)
            {
                combat_state.OnHitReaction();
            }
        }

        public static List<PlayerAgent> PlayerBotAgents
        {
            get
            {
                return PlayerAgentExtensions.GetAllBotPlayerAgents();
            }
        }
    }
}
