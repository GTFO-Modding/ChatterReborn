using ChatterReborn.Machines;
using ChatterReborn.Managers;
using HarmonyLib;
using Player;
using System;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(LockMelterFirstPerson))]
    class Patch_LockMelterFirstPerson
    {
        [HarmonyPatch(nameof(LockMelterFirstPerson.Setup))]
        static void Postfix(LockMelterFirstPerson __instance)
        {
            void OnTriggered(PlayerAgent playerAgent)
            {
                DramaChatterMachine dramaChatterMachine = DramaChatterManager.GetMachine(__instance.Owner);
                dramaChatterMachine?.CurrentState?.OnThrowConsumable(__instance);
            }
            __instance.m_interactApplyResource.add_OnInteractionTriggered(new Action<PlayerAgent>(OnTriggered));
        }
    }
}
