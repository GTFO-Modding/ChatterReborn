using Agents;
using ChatterReborn.Data;
using Enemies;

namespace ChatterReborn.ChatterEvent
{
    public struct EnemyDamageEvent
    {
        public EnemyAgent m_damageReceiver;

        public Agent m_attacker;

        public DamageType m_damageType;

        public bool m_killed;
    }
}
