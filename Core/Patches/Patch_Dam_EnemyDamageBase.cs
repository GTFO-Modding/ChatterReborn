using Agents;
using ChatterReborn.ChatterEvent;
using ChatterReborn.Data;
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
    [HarmonyPatch(typeof(Dam_EnemyDamageBase))]
    public class Patch_Dam_EnemyDamageBase
    {
        [HarmonyPatch(nameof(Dam_EnemyDamageBase.BulletDamage))]
        [HarmonyPostfix]
        static void OnBulletDamage(Dam_EnemyDamageBase __instance, float dam, Agent sourceAgent)
        {
            ChatterEventListenerHandler<EnemyDamageEvent>.PostEvent(new EnemyDamageEvent
            {
                m_attacker = sourceAgent,
                m_damageReceiver = __instance.Owner,
                m_killed = __instance.WillDamageKill(dam),
                m_damageType = DamageType.Bullet
            });
        }

        /*[HarmonyPatch(nameof(Dam_EnemyDamageBase.MeleeDamage))]
        [HarmonyPostfix]
        static void OnMeleeDamage(Dam_EnemyDamageBase __instance, float dam, Agent sourceAgent)
        {
            ChatterEventListenerHandler<EnemyDamageEvent>.PostEvent(new EnemyDamageEvent
            {
                m_attacker = sourceAgent,
                m_damageReceiver = __instance.Owner,
                m_killed = __instance.WillDamageKill(dam),
                m_damageType = DamageType.Melee
            });
        }*/
    }
}
