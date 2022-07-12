using ChatterReborn.Managers;
using ChatterReborn.Utils;
using HarmonyLib;
using Player;
using SNetwork;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(PlayerVoice))]
    class Patch_PlayerVoice
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerVoice.VoiceCallback))]
        static void Prefix_VoiceCallBack(PlayerVoice __instance)
        {
            if (__instance != null && __instance.m_owner != null)
            {
                if (__instance.m_dialogToStart > 0U && __instance.m_owner.IsBotOwned())
                {
                    PlayerDialogManager.WantToStartDialog(__instance.m_dialogToStart, __instance.m_playerID);
                }
            }
        }
    }
}
