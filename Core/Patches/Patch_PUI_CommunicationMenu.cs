using ChatterReborn.ChatterEvent;
using ChatterReborn.Data;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using HarmonyLib;
using Localization;
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
    [HarmonyPatch(typeof(PUI_CommunicationMenu))]
    class Patch_PUI_CommunicationMenu
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PUI_CommunicationMenu.Setup))]
        static void Post_Setup(PUI_CommunicationMenu __instance)
        {
            ExtraCommunicationManager.SetAsCurrentMenu(__instance);
        }


        [HarmonyPostfix]
        [HarmonyPatch(nameof(PUI_CommunicationMenu.OnCommunicationReceived))]
        static void Post_OnCommunicationReceived(SNet_Player src, uint textId, SNet_Player dst)
        {
            ChatterEventListenerHandler<TextCommandEvent>.PostEvent(new TextCommandEvent{
                source = src,
                textId = textId,
                destination = dst
            });
        }     


    }
}
