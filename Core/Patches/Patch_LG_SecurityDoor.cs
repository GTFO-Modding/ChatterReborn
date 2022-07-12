using ChatterReborn.Extra;
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
    [HarmonyPatch(typeof(LG_SecurityDoor))]
    class Patch_LG_SecurityDoor
    {
        [HarmonyPatch(nameof(LG_SecurityDoor.Setup))]
        static void Postfix(LG_SecurityDoor __instance)
        {
            if (__instance != null)
            {
                SecurityDialogDoor dialogdoor = new SecurityDialogDoor(__instance);
                __instance.m_sync.add_OnDoorStateChange(new Action<pDoorState, bool>(dialogdoor.OnOpenSecurityDoor));
            }
            else
            {
                ChatterDebug.LogError("No LG_SecurityDoor ???");
            }
        }
    }
}
