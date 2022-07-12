using ChatterReborn.Extra;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using HarmonyLib;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(LG_PickupItem_Sync))]
    class Patch_LG_PickupItem_Sync
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(LG_PickupItem_Sync.Setup))]
        static void Post_Setup(LG_PickupItem_Sync __instance)
        {
            if (__instance.item != null)
            {
                var descriptor = new LG_PickupItemDescriptor
                {
                    m_item = __instance.item
                };
                descriptor.Setup();
                __instance.add_OnSyncStateChange((Il2CppSystem.Action<ePickupItemStatus, pPickupPlacement, Player.PlayerAgent, bool>)descriptor.OnSyncState);
            }

        }
    }
}
