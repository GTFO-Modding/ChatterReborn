using ChatterReborn.Extra;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(StartMainGame))]
    class Patch_StartMainGame
    {

        [HarmonyPatch(nameof(StartMainGame.Start))]
        [HarmonyPostfix]
        static void OnStartGame()
        {

            ChatterRebornEntry.OnGameLoaded();
        }
    }
}
