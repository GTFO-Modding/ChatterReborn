using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using HarmonyLib;
using Player;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(PLOC_Downed))]
    class Patch_PLOC_Downed
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PLOC_Downed.Exit))]
        static void Post_Exit()
        {
            DramaChatterManager.CurrentState.Revived();
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(PLOC_Downed.SyncEnter))]
        static void Post_SyncEnter(PLOC_Downed __instance)
        {
            if (__instance.m_owner.IsBotOwned())
            {
                PlayerDialogManager.WantToStartDialog(GD.PlayerDialog.man_down_generic, -1, false);
            }
        }
    }
}
