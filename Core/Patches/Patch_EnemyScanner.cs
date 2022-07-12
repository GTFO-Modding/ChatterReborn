using ChatterReborn.Managers;
using GameData;
using Gear;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(EnemyScanner))]
    class Patch_EnemyScanner
    {
        /*[HarmonyPostfix]
        [HarmonyPatch(nameof(EnemyScanner.OnWield))]
        static void Post_OnWield(EnemyScanner __instance)
        {
            DES_Manager.OnWieldScanner(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(EnemyScanner.OnUnWield))]
        static void Post_OnUnWield()
        {
            DES_Manager.OnUnWieldScanner();
        }*/

        /*[HarmonyPrefix]
        [HarmonyPatch(nameof(EnemyScanner.Update))]
        static void Pre_Update(EnemyScanner __instance)
        {
            if (__instance != null && __instance.Owner != null && __instance.Owner.IsLocallyOwned)
            {
                if (!__instance.m_recharging)
                {
                    if (__instance.m_tagging && Clock.Time >= __instance.m_tagStartTime + EnemyScanner.TagDuration)
                    {
                        var taggable_enemies = __instance.m_taggableEnemies;
                        if (__instance.TryGetTaggableEnemies(20, __instance.FPItemHolder.ItemAimTrigger, out taggable_enemies))
                        {
                            if (taggable_enemies.Count > 1)
                            {
                                PrisonerDialogManager.WantToStartLocalDialogForced(GD.PlayerDialog.motion_detector_tagged_plural);
                            }
                        }
                    }
                }
            }     
        }*/

    }
}
