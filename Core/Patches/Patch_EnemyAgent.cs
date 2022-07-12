using ChatterReborn.Components;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using Enemies;
using HarmonyLib;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(EnemyAgent))]
    class Patch_EnemyAgent
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(EnemyAgent.Setup))]
        static void AfterSetup(EnemyAgent __instance, pEnemySpawnData spawnData)
        {
            EnemyDetectionManager.SetupEnemy(__instance, spawnData.mode);
            __instance.gameObject.AddAbsoluteComponent<EnemyDramaBehavior>();
        }
    }
}
