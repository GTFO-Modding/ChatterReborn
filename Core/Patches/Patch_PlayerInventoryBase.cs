using ChatterReborn.Utils;
using GameData;
using HarmonyLib;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(PlayerInventoryBase))]
    class Patch_PlayerInventoryBase
    {
        [HarmonyPatch(nameof(PlayerInventoryBase.DoReload))]
        static void Pre_DoReload(PlayerInventoryBase __instance)
        {
            if (__instance.m_wieldedItem != null)
            {
                if (__instance.Owner.Owner.IsBot && __instance.m_wieldedItem.WeaponComp.m_wasOutOfAmmo)
                {
                    __instance.Owner.WantToStartDialog(GD.PlayerDialog.on_reload_weapon_was_out, false);
                }
            }
            
        }
    }
}
