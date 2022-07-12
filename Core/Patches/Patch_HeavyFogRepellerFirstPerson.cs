using ChatterReborn.Extra;
using ChatterReborn.Managers;
using GameData;
using Gear;
using HarmonyLib;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(HeavyFogRepellerFirstPerson))]
    public class Patch_HeavyFogRepellerFirstPerson
    {
        [HarmonyPatch(nameof(HeavyFogRepellerFirstPerson.Setup))]
        public static void OnSetup(HeavyFogRepellerFirstPerson __instance, ItemDataBlock data)
        {
            DialogHeavyFogRepellerFirstPerson dialogRepeller = new DialogHeavyFogRepellerFirstPerson(__instance);
            DialogItemManager.Current.SetupPickUpInstance(__instance, dialogRepeller);
        }

        [HarmonyPatch(nameof(HeavyFogRepellerFirstPerson.OnWield))]
        public static void OnWielded(HeavyFogRepellerFirstPerson __instance)
        {
            DialogItemManager.OnWield(__instance);
        }
    }
}
