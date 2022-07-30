using Agents;
using ChatterReborn.ChatterEvent;
using ChatterReborn.Data;
using ChatterReborn.Utils;
using GameData;
using Player;
using System;
using System.Reflection;
using UnityEngine;

namespace ChatterReborn.Managers
{
    public class AgentDamageManager : ChatterManager<AgentDamageManager>
    {
        protected override void PostSetup()
        {
            Type[] bulletDmgTypes = new Type[]
            {
                typeof(float),
                typeof(Agent),
                typeof(Vector3),
                typeof(Vector3),
                typeof(Vector3),
                typeof(bool),
                typeof(int),
                typeof(float),
                typeof(float),
            };            
            
            Type[] meleeDmgTypes = new Type[]
            {
                typeof(float),
                typeof(Agent),
                typeof(Vector3),
                typeof(Vector3),
                typeof(int),
                typeof(float),
                typeof(float),
                typeof(float),
                typeof(float),
                typeof(bool),
                typeof(DamageNoiseLevel),
            };
            this.m_patcher.Patch<Dam_EnemyDamageBase>(nameof(Dam_EnemyDamageBase.BulletDamage), ChatterPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance, bulletDmgTypes);
            this.m_patcher.Patch<Dam_EnemyDamageBase>(nameof(Dam_EnemyDamageBase.MeleeDamage), ChatterPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance, meleeDmgTypes);
            this.m_patcher.Patch<Dam_PlayerDamageBase>(nameof(Dam_PlayerDamageBase.OnIncomingDamage), ChatterPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance);
            this.m_patcher.Patch<Dam_PlayerDamageBase>(nameof(Dam_PlayerDamageBase.ReceiveTentacleAttackDamage), ChatterPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance);
            this.m_patcher.Patch<Dam_SyncedDamageBase>(nameof(Dam_SyncedDamageBase.ShooterProjectileDamage), ChatterPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance);
            this.m_patcher.Patch<Dam_PlayerDamageBase>(nameof(Dam_PlayerDamageBase.ReceiveMeleeDamage), ChatterPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance);
            this.m_patcher.Patch<Dam_PlayerDamageLocal>(nameof(Dam_PlayerDamageLocal.ReceiveBulletDamage), ChatterPatchType.Prefix, BindingFlags.Public | BindingFlags.Instance);
            this.m_patcher.Patch<PlayerVoice>(nameof(PlayerVoice.VoiceCallback), ChatterPatchType.Prefix, BindingFlags.Public | BindingFlags.Instance);
        }

        private static void Dam_EnemyDamageBase__BulletDamage__Postfix(Dam_EnemyDamageBase __instance, float dam, Agent sourceAgent)
        {

            ChatterEventListenerHandler<EnemyDamageEvent>.PostEvent(new EnemyDamageEvent
            {
                m_attacker = sourceAgent,
                m_damageReceiver = __instance.Owner,
                m_killed = __instance.WillDamageKill(dam),
                m_damageType = DamageType.Bullet
            });
        }

        static void Dam_PlayerDamageLocal__ReceiveBulletDamage__Prefix(Dam_PlayerDamageLocal __instance)
        {
            __instance.Owner.WantToStartDialog(GD.PlayerDialog.friendly_fire_outburst, true);
            __instance.m_damageVoiceTimer = Clock.Time + 2f;
        }


        static void PlayerVoice__VoiceCallback__Prefix(PlayerVoice __instance)
        {
            if (__instance != null && __instance.m_owner != null)
            {
                if (__instance.m_dialogToStart > 0U && __instance.m_owner.IsBotOwned())
                {
                    PlayerDialogManager.WantToStartDialog(__instance.m_dialogToStart, __instance.m_playerID);
                }
            }
        }

        private static void Dam_EnemyDamageBase__MeleeDamage__Postfix(Dam_EnemyDamageBase __instance, float dam, Agent sourceAgent)
        {
            ChatterEventListenerHandler<EnemyDamageEvent>.PostEvent(new EnemyDamageEvent
            {
                m_attacker = sourceAgent,
                m_damageReceiver = __instance.Owner,
                m_killed = __instance.WillDamageKill(dam),
                m_damageType = DamageType.Melee
            });
        }

        private static void Dam_PlayerDamageBase__OnIncomingDamage__Postfix(Dam_PlayerDamageBase __instance, float damage)
        {
            ChatterEventListenerHandler<PlayerDamageEvent>.PostEvent(new PlayerDamageEvent
            {
                damageAmount = damage,
                damageReceiver = __instance.Owner,
            });
        }

        private static void Dam_PlayerDamageBase__ReceiveTentacleAttackDamage__Postfix(Dam_PlayerDamageBase __instance, pMediumDamageData data)
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

        private static void Dam_SyncedDamageBase__ShooterProjectileDamage__Postfix(Dam_PlayerDamageBase __instance, float dam, Vector3 position)
        {
            OnHitReactBot(__instance.Owner);
        }

        private static void Dam_PlayerDamageBase__ReceiveMeleeDamage__Postfix(Dam_PlayerDamageBase __instance, pMediumDamageData data)
        {
            OnHitReactBot(__instance.Owner);
        }

        private static void OnHitReactBot(PlayerAgent playerAgent)
        {
            if (playerAgent.IsBotOwned())
            {
                DialogBotManager.OnHitReactBot(playerAgent);
            }
        }
    }
}
