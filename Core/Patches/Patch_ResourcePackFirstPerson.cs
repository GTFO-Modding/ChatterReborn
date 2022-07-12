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
    [HarmonyPatch(typeof(ResourcePackFirstPerson), "Setup")]
    public class Patch_ResourcePackFirstPerson
    {
        public static void Postfix(ResourcePackFirstPerson __instance)
        {
            ResourceFirstPersonDialog pack = new ResourceFirstPersonDialog(__instance);
            DialogItemManager.Current.SetupPickUpInstance(__instance, pack);
        }
    }
}
