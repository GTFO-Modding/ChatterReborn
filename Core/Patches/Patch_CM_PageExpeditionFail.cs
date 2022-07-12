using CellMenu;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(CM_PageExpeditionFail))]
    class Patch_CM_PageExpeditionFail
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(CM_PageExpeditionFail.OnEnable))]

        static void Post_OnEnable(CM_PageExpeditionFail __instance)
        {
            if (__instance.m_isSetup)
            {
                if (ConfigurationManager.ExpeditionFailedDeathScreamEnabled)
                {
                    PrisonerDialogManager.DelayLocalDialogForced(UnityEngine.Random.Range(1f, 3f), GameData.GD.PlayerDialog.death_scream);
                    
                }
                ChatterDebug.LogWarning("<<<<<<<<<DEATH SCREAM>>>>>>>>>>");
            }            
        }
    }
}
