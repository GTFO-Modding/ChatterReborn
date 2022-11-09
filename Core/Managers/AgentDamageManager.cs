﻿using Agents;
using ChatterReborn.Attributes;
using ChatterReborn.ChatterEvent;
using ChatterReborn.Data;
using ChatterReborn.Utils;
using ChatterRebornTokens;
using GameData;
using HarmonyLib;
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
            this.m_patcher.Patch<Dam_EnemyDamageBase>(nameof(Dam_EnemyDamageBase.BulletDamage), HarmonyPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance, bulletDmgTypes);
            this.m_patcher.Patch<Dam_EnemyDamageBase>(nameof(Dam_EnemyDamageBase.MeleeDamage), HarmonyPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance, meleeDmgTypes);
            this.m_patcher.Patch<Dam_PlayerDamageBase>(nameof(Dam_PlayerDamageBase.OnIncomingDamage), HarmonyPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance);
            this.m_patcher.Patch<Dam_PlayerDamageBase>(nameof(Dam_PlayerDamageBase.ReceiveTentacleAttackDamage), HarmonyPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance);
            this.m_patcher.Patch<Dam_SyncedDamageBase>(nameof(Dam_SyncedDamageBase.ShooterProjectileDamage), HarmonyPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance);
            this.m_patcher.Patch<Dam_PlayerDamageBase>(nameof(Dam_PlayerDamageBase.ReceiveMeleeDamage), HarmonyPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance);
            this.m_patcher.Patch<Dam_PlayerDamageLocal>(nameof(Dam_PlayerDamageLocal.ReceiveBulletDamage), HarmonyPatchType.Prefix, BindingFlags.Public | BindingFlags.Instance);
            this.m_patcher.Patch<PlayerVoice>(nameof(PlayerVoice.VoiceCallback), HarmonyPatchType.Prefix, BindingFlags.Public | BindingFlags.Instance);
        }


        [MethodDecoderToken]
        private static void Dam_EnemyDamageBase__BulletDamage__Postfix(Dam_EnemyDamageBase __instance, float dam, Agent sourceAgent, Vector3 position, Vector3 direction, Vector3 normal, bool allowDirectionalBonus = false, int limbID = 0, float staggerMulti = 1f, float precisionMulti = 1f)
        {
            ChatterEventListenerHandler<EnemyDamageEvent>.PostEvent(new EnemyDamageEvent
            {
                m_attacker = sourceAgent,
                m_damageReceiver = __instance.Owner,
                m_killed = __instance.WillDamageKill(dam),
                m_damageType = DamageType.Bullet
            });
        }


        [MethodDecoderToken]
        static void Dam_PlayerDamageLocal__ReceiveBulletDamage__Prefix(Dam_PlayerDamageLocal __instance)
        {
            __instance.Owner.WantToStartDialog(GD.PlayerDialog.friendly_fire_outburst, true);
            __instance.m_damageVoiceTimer = Clock.Time + 2f;
        }

        [MethodDecoderToken]
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

        [MethodDecoderToken]
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

        [MethodDecoderToken]
        private static void Dam_PlayerDamageBase__OnIncomingDamage__Postfix(Dam_PlayerDamageBase __instance, float damage)
        {
            ChatterEventListenerHandler<PlayerDamageEvent>.PostEvent(new PlayerDamageEvent
            {
                damageAmount = damage,
                damageReceiver = __instance.Owner,
            });
        }


        [MethodDecoderToken]
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


        [MethodDecoderToken]
        private static void Dam_SyncedDamageBase__ShooterProjectileDamage__Postfix(Dam_PlayerDamageBase __instance, float dam, Vector3 position)
        {
            OnHitReactBot(__instance.Owner);
        }


        [MethodDecoderToken]
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
