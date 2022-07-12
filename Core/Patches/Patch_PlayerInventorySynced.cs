using ChatterReborn.Managers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(PlayerInventorySynced))]
    class Patch_PlayerInventorySynced
    {
        [HarmonyPatch(nameof(PlayerInventorySynced.SyncWieldItem))]
        static void Postfix(ItemEquippable item)
        {
            DramaChatterManager.OtherPlayerSyncWield(item);
        }
    }
}
