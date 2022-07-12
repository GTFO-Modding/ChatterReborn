using ChatterReborn.Extra;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameEvent;
using HarmonyLib;
using LevelGeneration;
using Player;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Patches
{

    [HarmonyPatch(typeof(LG_WeakDoor))]
    class Patch_LG_WeakDoor
    {
        [HarmonyPatch(nameof(LG_WeakDoor.Setup))]
        [HarmonyPostfix]
        public static void AfterSetup(LG_WeakDoor __instance)
        {
            if (__instance != null)
            {
                WeakDialogDoor dialogdoor = new WeakDialogDoor(__instance);
                __instance.m_sync.add_OnDoorStateChange(new Action<pDoorState, bool>(dialogdoor.OnOpenWeakDoor));

                __instance.m_sync.add_OnDoorGotDamage(new Action<float, float, bool, bool, SNet_Player>(dialogdoor.OnRecieveDamage));
            }
            else
            {
                ChatterDebug.LogError("No LG_WeakDoor ???");
            }

        }


    }
}
