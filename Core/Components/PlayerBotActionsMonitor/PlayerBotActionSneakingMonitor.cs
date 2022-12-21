using ChatterReborn.Managers;
using ChatterReborn.Utils;
using Enemies;
using GameData;
using UnityEngine;

namespace ChatterReborn.Components.PlayerBotActionsMonitor
{
    public class PlayerBotSneakingMonitor : PlayerBotMonitorBase
    {
        public PlayerBotSneakingMonitor(PlayerBotAIRootMonitor aiActionMonitor) : base(aiActionMonitor)
        {
        }


        private bool GetSleeperFromPosition(Vector3 position, out PlayerBotSneakingCallOutOption targetOption)
        {
            targetOption = default;
            bool hasOption = false;
            if (this.BotAgent.CourseNode == null)
            {
                return false;
            }
            var enemies = Il2cppUtils.ToSystemList(AIGraph.AIG_CourseGraph.GetReachableEnemiesInNodes(this.BotAgent.CourseNode, 2));
            float closestDistance = 0f;
            foreach (var enemyAgent in enemies)
            {
                var distance = Vector3.Distance(enemyAgent.Position, position);
                if (distance < sneakingRadiusCheck)
                {
                    uint enemySpottedDialogId = enemyAgent.EnemyData.EnemySpottedDialogId;
                    bool hasEnemySpottedDialog = enemySpottedDialogId > 0;
                    bool isEnemyScout = enemyAgent.IsScout;
                    var newOption = targetOption = new PlayerBotSneakingCallOutOption
                    {
                        Enemy = enemyAgent,
                        Prio = isEnemyScout ? 2 : 1,
                        CallOutID = hasEnemySpottedDialog ? enemySpottedDialogId : GD.PlayerDialog.spot_idle_guard_group
                    };
                    if (!hasOption || closestDistance > distance && targetOption.Prio < newOption.Prio)
                    {                        
                        if (enemyAgent.Locomotion.CurrentStateEnum == ES_StateEnum.Hibernate || hasEnemySpottedDialog)
                        {
                            if (this.Bot.CanSeeObject(this.BotAgent.EyePosition, enemyAgent.gameObject))
                            {
                                closestDistance = distance;
                                hasOption = true;
                                targetOption = newOption;
                            }
                        }                                            
                    }
                }
            }
            return hasOption;
        }


        public override void UpdateMonitor()
        {
            if (!IsLocallyOwned)
            {
                return;
            }

            if (this.Bot.m_hasSleeperNearby && !EnemyDetectionManager.AnyEnemiesAlerted)
            {
                if (this.GetSleeperFromPosition(this.Bot.m_lastSleeperCheckPosition, out PlayerBotSneakingCallOutOption targetOption))
                {
                    if (!m_firstTriggerDialog || m_lastTriggerDialog < Time.time)
                    {
                        m_firstTriggerDialog = true;
                        m_lastTriggerDialog = Time.time + 1f;
                        BotAgent.WantToStartDialog(targetOption.CallOutID, false);
                    }
                }

            }
        }



        public static float sneakingRadiusCheck = 35f;

        private float m_lastTriggerDialog;

        private bool m_firstTriggerDialog;

        

        private struct PlayerBotSneakingCallOutOption
        {
            public EnemyAgent Enemy;
            public int Prio;
            public uint CallOutID;
        }
    }
}
