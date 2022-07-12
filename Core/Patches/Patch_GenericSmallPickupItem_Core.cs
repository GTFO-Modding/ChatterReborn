using ChatterReborn.Extra;
using ChatterReborn.Managers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(GenericSmallPickupItem_Core))]
    class Patch_GenericSmallPickupItem_Core
    {
        [HarmonyPatch(nameof(GenericSmallPickupItem_Core.SetupFromLevelgen))]
        static void Postfix(GenericSmallPickupItem_Core __instance)
        {
            var dialogitem = new GenericSmallItemPickUp(__instance);
            DialogItemManager.Current.SetupPickUpInstance(__instance, dialogitem);
        }
    }
}
