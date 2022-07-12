using ChatterReborn.ChatterEvent;
using HarmonyLib;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{

    //[HarmonyPatch(typeof(WardenObjectiveManager))]
    class Patch_WardenObjectiveManager
    {
        /*[HarmonyPostfix]
        [HarmonyPatch(nameof(WardenObjectiveManager.CheckWardenObjectiveStatus))]
        static void Post_CheckWardenObjectiveStatus(bool isRecall, LG_LayerType layer, pWardenObjectiveState newState, eWardenObjectiveStatus oldStatus, eWardenObjectiveStatus newStatus, int oldIndex, int newIndex, eWardenSubObjectiveStatus oldSub, eWardenSubObjectiveStatus newSub)
        {
            WardenObjectiveStatus wardenObjectiveStatus = new WardenObjectiveStatus(isRecall, layer, newState, oldStatus, newStatus, oldIndex, newIndex, oldSub, newSub);
            ChatterEvent.ChatterEventListenerHandler<WardenObjectiveStatus>.PostEvent(wardenObjectiveStatus);
        }*/
    }
}
