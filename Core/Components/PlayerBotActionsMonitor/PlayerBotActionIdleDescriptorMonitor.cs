using ChatterReborn.Utils;
using Enemies;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Components.PlayerBotActionsMonitor
{
    public class PlayerBotActionIdleDescriptorMonitor : PlayerBotActionDescriptorMonitorBase<PlayerBotActionIdle, PlayerBotActionIdle.Descriptor>
    {
        public PlayerBotActionIdleDescriptorMonitor(PlayerBotAIRootMonitor aiActionMonitor) : base(aiActionMonitor)
        {
            this.m_actionDesc = aiActionMonitor.m_rootAction.m_idleAction;
        }

        public override void UpdateMonitor()
        {
            if (this.GetActionBase != null)
            {
                var actionBase = this.GetActionBase;

                if (actionBase.m_lookAction.TargetObj != null)
                {
                    var enemyAgent = actionBase.m_lookAction.TargetObj.gameObject.GetAbsoluteComponent<EnemyAgent>();
                    if (enemyAgent != null)
                    {
                        m_lookAtEnemy = enemyAgent;
                        m_hasLookObject = true;
                        return;
                    }
                }
            }


            m_hasLookObject = false;
            m_lookAtEnemy = null;
        }

        private bool m_hasLookObject;

        private EnemyAgent m_lookAtEnemy;

        
        public EnemyAgent LookTarget => m_hasLookObject ? m_lookAtEnemy : null;
    }
}
