using ChatterReborn.Managers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(ThrowingWeapon))]
    class Patch_ThrowingWeapon
    {
        [HarmonyPatch(nameof(ThrowingWeapon.Throw))]
        static void Postfix(ThrowingWeapon __instance)
        {

            DramaChatterManager.GetMachine(__instance.Owner)?.CurrentState?.OnThrowConsumable(__instance);

        }
    }
}
