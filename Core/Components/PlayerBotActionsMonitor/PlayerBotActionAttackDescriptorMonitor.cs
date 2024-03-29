﻿using ChatterReborn.Data;
using ChatterReborn.Drama_Chatter_States;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using Enemies;
using GameData;
using Player;
using UnityEngine;

namespace ChatterReborn.Components.PlayerBotActionsMonitor
{
    public class PlayerBotActionAttackDescriptorMonitor : PlayerBotActionDescriptorMonitorBase<PlayerBotActionAttack, PlayerBotActionAttack.Descriptor>
    {
        public PlayerBotActionAttackDescriptorMonitor(PlayerBotAIRootMonitor aiActionMonitor) : base(aiActionMonitor)
        {
            this.m_actionDesc = aiActionMonitor.m_rootAction.m_attackAction;
            this.CallOutMonsterDel = AttemptToCallOutSpecialMonster_Main;
        }

        private DRAMA_Chatter_Combat CombatState => (DRAMA_Chatter_Combat)this.m_rootMonitor.ChatterMachine.GetState(DRAMA_State.Combat);

        public override void UpdateMonitor()
        {
            if (this.m_actionDesc.Status == PlayerBotActionBase.Descriptor.StatusType.Active)
            {
                if (DramaManager.CurrentStateEnum == DRAMA_State.Combat || DramaManager.CurrentStateEnum == DRAMA_State.IntentionalCombat || DramaManager.CurrentStateEnum == DRAMA_State.Survival || DramaManager.CurrentStateEnum == DRAMA_State.Encounter)
                {
                    if (IsLocallyOwned)
                    {
                        UpdateCombat();
                    }
                }
            }
        }

        private void UpdateCombat()
        {
            UpdateCombatChatter();
            UpdateSpecialCallOuts();
        }

        private void UpdateCombatChatter()
        {
            if (ConfigurationManager.PersistentCombatChatterEnabled && ConfigurationManager.AllowBotsToParticipateEnabled)
            {
                CombatState.UpdateCombatChatter();
            }
        }

        private void UpdateSpecialCallOuts()
        {
            if (this.GetActionBase != null)
            {
                var actionBase = this.GetActionBase;
                if (actionBase.m_currentAttackOption != null)
                {
                    var attackOption = actionBase.m_currentAttackOption;
                    if (attackOption.TargetAgent != null && attackOption.TargetAgent.Convert(out EnemyAgent enemyAgent))
                    {
                        if (m_rootMonitor.m_bot.CanSeeObject(this.BotAgent.EyePosition, attackOption.TargetAgent.gameObject))
                        {
                            if (CallOutMonsterDel != null)
                            {
                                CallOutMonsterDel(enemyAgent);
                            }
                            
                        }
                    }
                }
            }
        }

        private delegate void FuncClkMonster(EnemyAgent enemyAgent);

        private FuncClkMonster CallOutMonsterDel { get; set; }



        private void AttemptToCallOutSpecialMonster_Main(EnemyAgent enemyAgent)
        {

            CallOutEnemyTarget targetCallOut = new CallOutEnemyTarget();            
            if (enemyAgent.RequireTagForDetection)
            {               
                if (!enemyAgent.IsTagged)
                {
                    targetCallOut.Recognize(EnemyFilter.Shadow);
                }                
            }
            AttemptToCallOut(targetCallOut);
        }

        private bool DoesTeamHaveEnemyScanner
        {
            get
            {
                for (int i = 0; i < ExtendedPlayerManager.AllPlayersInLevel.Count; i++)
                {
                    PlayerAgent player = ExtendedPlayerManager.AllPlayersInLevel[i];
                    if (player != this.BotAgent && player.Alive)
                    {
                        var backPack = PlayerBackpackManager.GetBackpack(player.Owner);

                        if (backPack != null && backPack.TryGetBackpackItem(InventorySlot.GearClass, out var backpackItem) && backpackItem.ItemID == GD.Item.GEAR_MotionTracker)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        private bool DoesHaveEnemyScanner
        {
            get
            {
                var backPack = PlayerBackpackManager.GetBackpack(this.BotAgent.Owner);
                if (backPack != null && backPack.TryGetBackpackItem(InventorySlot.GearClass, out var backpackItem) && backpackItem.ItemID == GD.Item.GEAR_MotionTracker)
                {
                    return true;
                }
                return false;
            }
        }
        private void AttemptToCallOut(CallOutEnemyTarget targetCallOut)
        {
            if (targetCallOut.IsRecognizedAs(EnemyFilter.Shadow))
            {
                if (DoesTeamHaveEnemyScanner && !DoesHaveEnemyScanner)
                {
                    WeightHandler<uint> handler = WeightHandler<uint>.CreateWeightHandler();                    
                    if (this.BotAgent.PlayerCharacterFilter != DialogCharFilter.Char_T)
                    {
                        handler.AddValue(GD.PlayerDialog.CL_TagThem, 3f);                        
                    }
                    handler.AddValue(GD.PlayerDialog.CL_Scan, 2f);
                    CallOutTargetEnemy(handler.Best.Value);
                }                
            }
            else if (targetCallOut.IsRecognizedAs(EnemyFilter.Big))
            {
                CallOutTargetEnemy(GD.PlayerDialog.CL_BigGuyHere);
            }
            else if (targetCallOut.IsRecognizedAs(EnemyFilter.Flyer))
            {
                CallOutTargetEnemy(GD.PlayerDialog.CL_LookUp);
            }
        }

        private void CallOutTargetEnemy(uint calloutID)
        {
            if (m_firstSpecialCallOut && m_specialCallOutTimer > Time.time)
            {
                return;
            }

            if (PlayerBotAIRootMonitor.PlayerBotSpecialCallOutTimer > Time.time)
            {
                return;
            }

            m_firstSpecialCallOut = true;
            m_specialCallOutTimer = Time.time + 2f;
            PlayerBotAIRootMonitor.PlayerBotSpecialCallOutTimer = Time.time + 8f;
            this.BotAgent.WantToStartDialog(calloutID, true);
        }



        private bool m_firstSpecialCallOut;

        private float m_specialCallOutTimer;

        private class CallOutEnemyTarget
        {
            private EnemyFilter m_enemyFilter;

            public void Recognize(EnemyFilter enemyFilter)
            {
                m_enemyFilter |= enemyFilter;
            }

            public bool IsRecognizedAs(EnemyFilter enemyFilter)
            {
                return this.m_enemyFilter.HasFlag(enemyFilter);
            }
        }
        
    }
}
