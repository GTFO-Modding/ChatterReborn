using ChatterReborn.Utils;
using GameData;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;




namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(Dam_PlayerDamageLocal))]
    class Patch_PlayerDamageLocal
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Dam_PlayerDamageLocal.ReceiveBulletDamage))]
        static void Pre_ReceiveBulletDamage(Dam_PlayerDamageLocal __instance)
        {
            __instance.Owner.WantToStartDialog(GD.PlayerDialog.friendly_fire_outburst, true);
            __instance.m_damageVoiceTimer = Clock.Time + 2f;
        }


        /*[HarmonyPrefix]
        [HarmonyPatch(nameof(Dam_PlayerDamageLocal.Hitreact))]
        static void Pre_Hitreact(float damage, Vector3 direction, ref bool triggerCameraShake, ref bool triggerGenericDialog)
        {
            if (IgnoreNextDialogTrigger)
            {
                triggerGenericDialog = false;
                IgnoreNextDialogTrigger = false;
            }
        }

        static bool IgnoreNextDialogTrigger = false;*/
    }
}
