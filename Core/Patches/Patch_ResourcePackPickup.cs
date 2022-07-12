using ChatterReborn.Extra;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using Gear;
using HarmonyLib;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(ResourcePackPickup), "Setup")]
    public class Patch_ResourcePackPickup
    {
        public static void Postfix(ResourcePackPickup __instance)
        {
            ResourcePickUpDialog pack = new ResourcePickUpDialog(__instance);
            DialogItemManager.Current.SetupPickUpInstance(__instance, pack);
        }
    }
}
