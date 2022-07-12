using ChatterReborn.Managers;
using HarmonyLib;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(DramaManager))]
    class Patch_DramaManager
    {
        [HarmonyPatch(nameof(DramaManager.ChangeState))]
        [HarmonyPostfix]
        static void OnChangeState(DRAMA_State state, bool doSync)
        {
            DramaChatterManager.OnChangeState(state, doSync);
        }

        [HarmonyPatch(nameof(DramaManager.CheckSyncedPlayerStates))]
        [HarmonyPostfix]
        static void AfterCheckSyncedPlayerStates(out bool hasCombat, out bool hasEncounter, out bool hasSneaking)
        {
            hasCombat = false;
            hasEncounter = false;
            hasSneaking = false;
            if (SNet.HasMaster)
            {
                DRAMA_State master_DramaState = DramaManager.SyncedPlayerStates[SNet.Master.CharacterIndex];
                if (master_DramaState == DRAMA_State.Encounter)
                {
                    hasEncounter = true;
                }
                if (master_DramaState == DRAMA_State.Combat || master_DramaState == DRAMA_State.IntentionalCombat || master_DramaState == DRAMA_State.Survival)
                {
                    hasCombat = true;
                }
                if (master_DramaState == DRAMA_State.Sneaking)
                {
                    hasSneaking = true;
                }
            }
        }

        /*[HarmonyPatch(nameof(DramaManager.GoToSneaking), MethodType.Getter)]
        [HarmonyPostfix]
        static void OverrideGoToSneaking(ref bool __result)
        {
            __result = DialogEnemyManager.EnemiesAlerted <= 0 && DialogEnemyManager.EnemiesCloseBy;
        }*/
    }
}
