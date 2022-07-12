using ChatterReborn.Data;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(PLOC_GrabbedByPouncer))]
    class Patch_PLOC_GrabbedByPouncer
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(PLOC_GrabbedByPouncer.Enter))]
        static void Pre_Enter(PLOC_GrabbedByPouncer __instance)
        {
            OnGrabbedByPouncerDialog(__instance);
        }
        [HarmonyPrefix]
        [HarmonyPatch(nameof(PLOC_GrabbedByPouncer.SyncEnter))]
        static void Pre_SyncEnter(PLOC_GrabbedByPouncer __instance)
        {
            if (__instance.m_owner.IsBotOwned())
            {
                OnGrabbedByPouncerDialog(__instance);
            }
        }

        private static void OnGrabbedByPouncerDialog(PLOC_GrabbedByPouncer instance)
        {
            instance.m_owner.WantToStartDialog(GameData.GD.PlayerDialog.on_grabbed_by_tank, true);
            instance.m_dialogTimer = Time.time + UnityEngine.Random.Range(3f, 5f);
        }


        /*[HarmonyPrefix]
        [HarmonyPatch(nameof(PLOC_GrabbedByPouncer.CommonUpdate))]
        static void Pre_CommonUpdate(PLOC_GrabbedByPouncer __instance)
        {
            
            if (__instance != null && __instance.m_owner != null && __instance.m_dialogTimer < Time.time)
            {
                __instance.m_dialogTimer = Time.time + UnityEngine.Random.Range(8f, 12f);
                __instance.m_owner.WantToStartDialog(GameData.GD.PlayerDialog.held_by_tank, true);
            }
            
        }*/

    }
}
