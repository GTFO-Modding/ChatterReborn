using ChatterReborn.Managers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(InfectionSpitter))]
    class Patch_InfectionSpitter
    {
        [HarmonyPatch(nameof(InfectionSpitter.DoExplode))]

        static void Postfix(InfectionSpitter __instance)
        {
            DramaChatterManager.CurrentState.OnSpitterExplode(__instance);
        }
    }
}
