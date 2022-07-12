using ChatterReborn.Extra;
using ChatterReborn.Managers;
using HarmonyLib;
using LevelGeneration;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(GateKeyItem))]
    class Patch_GateKeyItem
    {
        [HarmonyPatch(nameof(GateKeyItem.Setup))]
        static void Postfix(GateKeyItem __instance)
        {
            var dialogitem = new KeyItemPickUpDialog(__instance.keyPickupCore);
            DialogItemManager.Current.SetupPickUpInstance(__instance.keyPickupCore, dialogitem);
        }
    }
}
