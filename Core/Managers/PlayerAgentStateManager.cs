using ChatterReborn.Utils;
using GameData;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Managers
{
    public class PlayerAgentStateManager : ChatterManager<PlayerAgentStateManager>
    {

        protected override void PostSetup()
        {
            this.m_patcher.Patch<PLOC_Downed>(nameof(PLOC_Downed.SyncEnter), Data.ChatterPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            this.m_patcher.Patch<PLOC_Downed>(nameof(PLOC_Downed.CommonExit), Data.ChatterPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }

        static void PLOC_Downed__CommonExit__Postfix(PLOC_Downed __instance)
        {
            var machine = DramaChatterManager.GetMachine(__instance.m_owner);
            if (machine != null)
            {
                machine.CurrentState.Revived();
            }            
        }

        static void PLOC_Downed__SyncEnter__Postfix(PLOC_Downed __instance)
        {
            if (__instance.m_owner.IsBotOwned())
            {
                PlayerDialogManager.WantToStartDialog(GD.PlayerDialog.man_down_generic, -1, false);
            }
        }
    }
}
