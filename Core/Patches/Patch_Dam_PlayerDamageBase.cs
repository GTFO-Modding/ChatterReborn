using ChatterReborn.ChatterEvent;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using HarmonyLib;
using Player;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(Dam_PlayerDamageBase))]
    class Patch_Dam_PlayerDamageBase
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Dam_PlayerDamageBase.OnIncomingDamage))]
        static void Post_OnIncomingDamage(Dam_PlayerDamageBase __instance, float damage)
        {
            ChatterEventListenerHandler<PlayerDamageEvent>.PostEvent(new PlayerDamageEvent
            {
                damageAmount = damage,
                damageReceiver = __instance.Owner,
            });
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Dam_PlayerDamageBase.ReceiveTentacleAttackDamage))]
        static void Post_ReceiveTentacleAttackDamage(Dam_PlayerDamageBase __instance, pMediumDamageData data)
        {
            if (__instance.Owner.IsBotOwned())
            {
                uint dialogID = GD.PlayerDialog.after_damage_generic;
                if ((__instance.Owner.Inventory.WieldedSlot == InventorySlot.GearStandard || __instance.Owner.Inventory.WieldedSlot == InventorySlot.GearSpecial) && !__instance.Owner.Inventory.HasAmmoInEquipped())
                {
                    dialogID = GD.PlayerDialog.ammo_depleted_taking_damage;
                }
                PlayerVoiceManager.WantToSayAndStartDialog(__instance.Owner.CharacterID, EVENTS.MELEETENTACLEHITPLAYER, dialogID);
            }

            
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Dam_PlayerDamageBase.ReceiveShooterProjectileDamage))]
        static void Post_ReceiveShooterProjectileDamage(Dam_PlayerDamageBase __instance, pMediumDamageData data)
        {
            if (__instance.Owner.IsBotOwned())
            {
                DialogBotManager.OnHitReactBot(__instance.Owner);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Dam_PlayerDamageBase.ReceiveMeleeDamage))]
        static void Post_ReceiveMeleeDamage(Dam_PlayerDamageBase __instance, pMediumDamageData data)
        {
            if (__instance.Owner.IsBotOwned())
            {
                DialogBotManager.OnHitReactBot(__instance.Owner);
            }
        }
    }
}
