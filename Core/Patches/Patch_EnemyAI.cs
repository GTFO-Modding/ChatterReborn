using Agents;
using ChatterReborn.Managers;
using Enemies;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(EnemyAI))]
    class Patch_EnemyAI
    {
        [HarmonyPatch(nameof(EnemyAI.ModeChange))]
        static void Postfix(EnemyAI __instance)
        {
            if (__instance != null && __instance.m_enemyAgent != null)
            {
                EnemyDetectionManager.SetupEnemy(__instance.m_enemyAgent, __instance.Mode);
            }

        }
    }
}
