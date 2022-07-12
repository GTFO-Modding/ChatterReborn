using Agents;
using ChatterReborn.Data;
using Enemies;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
