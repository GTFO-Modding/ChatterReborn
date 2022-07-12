using ChatterReborn.Managers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(HackingMinigame_TimingGrid))]
    class Patch_HackingMinigame_TimingGrid
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(HackingMinigame_TimingGrid.StartGame))]
        static void Post_SetupGame(HackingMinigame_TimingGrid __instance)
        {
            HackingManager.Current.SetupCurrentMiniGame(__instance);
        }


        [HarmonyPostfix]
        [HarmonyPatch(nameof(HackingMinigame_TimingGrid.EndGame))]
        static void Post_EndGame()
        {
            HackingManager.Current.EndGame();
        }

    }
}
