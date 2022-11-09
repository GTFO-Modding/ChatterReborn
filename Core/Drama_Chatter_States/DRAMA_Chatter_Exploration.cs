using ChatterReborn.Data;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using ChatterRebornSettings;
using GameData;
using Player;

namespace ChatterReborn.Drama_Chatter_States
{
    public class DRAMA_Chatter_Exploration : DRAMA_Chatter_Base
    {
        public override void Setup()
        {
            this.m_idleDialogCallback = new CallBackUtils.CallBack(TriggerIdleDialog);
            this.m_idle_delay = new MinMaxTimer(20f, 45f);
            this.m_triggerLightOnDarkCallBack = new CallBackUtils.CallBack<ItemEquippable>(this.TriggerLightOnDarkDialogue);
            base.Setup();
        }


        public override void OtherPlayerSyncWield(ItemEquippable equipment)
        {
            if (equipment.ItemDataBlock.persistentID == GD.Item.CONSUMABLE_FlashlightMedium)
            {
                this.m_triggerLightOnDarkCallBack.QueueCallBack(new MinMaxTimer(1f, 2f), equipment);
            }
        }


        private void StartDelayedIdleDialog()
        {
            DelayValue delay = this.m_idle_delay;
            this.m_idleDialogCallback.QueueCallBack(delay);
            this.DebugPrint("Now delaying idle dialogs for " + delay.totalTimer + " seconds.");
        }

        public override void Enter()
        {
            this.CommonEnter();
        }

        public override void SyncEnter()
        {
            this.CommonEnter();
        }

        private void CommonEnter()
        {
            this.StartDelayedIdleDialog();
        }

        public override void OnSpitterExplode(InfectionSpitter infectionSpitter)
        {
            DelayValue randomDelay = new MinMaxTimer(0.25f, 0.5f);
            this.m_spitter_explodeCallBack.QueueCallBack(infectionSpitter.m_currentExplodeSpeed + randomDelay, infectionSpitter);
        }

        private void CommonExit()
        {
            this.m_idleDialogCallback.RemoveCallBack();
        }

        public override void Exit()
        {
            this.CommonExit();
        }

        public override void SyncExit()
        {
            this.CommonExit();
        }


        public override void Update()
        {
            this.TryToOrderBackPlayer();
        }


        private void TriggerLightOnDarkDialogue(ItemEquippable equipment)
        {
            if (equipment == null || !equipment.HasFlashlight || !equipment.AttachedFlashlight.IsEnabled)
            {
                return;
            }



            PlayerAgent playerAgent = this.m_owner;
            PlayerAgent teammate = equipment.Owner;

            if (playerAgent != null && playerAgent.Alive && teammate != null && teammate.Alive)
            {

                if (!ExtendedPlayerManager.LightsAvailable(playerAgent))
                {
                    float magnitude = (teammate.Position - playerAgent.Position).magnitude;
                    if (magnitude < 10f && ConfigurationManager.FlashLightInDarknessDialogueEnabled)
                    {
                        float delay = UnityEngine.Random.Range(0.35f, 0.75f);
                        this.m_triggerDialogCallback.QueueCallBack(delay, GD.PlayerDialog.dark_area_light_on, false);
                    }
                }

            }
        }


        private void CleanUp()
        {
            this.m_idleDialogCallback.RemoveCallBack();
            this.m_triggerLightOnDarkCallBack.RemoveCallBack();
        }

        public override void OnDestroyed()
        {
            this.CleanUp();
            base.OnDestroyed();
        }
        public override void OnLevelCleanUp()
        {
            this.CleanUp();
            base.OnLevelCleanUp();
        }




        private void TriggerIdleDialog()
        {
            if (this.HasOwner && this.Owner.Alive && this.m_machine.AllowedToParticipate)
            {
                WeightHandler<uint> handler = WeightHandler<uint>.CreateWeightHandler();
                bool isStable = true;
                float healthRel = this.Owner.Damage.GetHealthRel();
                if (healthRel < 0.4f)
                {
                    isStable = false;
                    if (ConfigurationManager.MedicCommentsEnabled)
                    {
                        string key = CoolDownType.IdleLowHealth.ToString() + "_" + this.Owner.CharacterID;
                        if (!CoolDownManager.HasCooldown(key))
                        {
                            handler.AddValue(GD.PlayerDialog.idle_low_health, 3f);
                            CoolDownManager.ApplyCooldown(key, new MinMaxTimer(60f, 180f));
                        }                        
                        if (healthRel < 0.25f)
                        {
                            handler.AddValue(GD.PlayerDialog.low_health_talk, 2f);
                        }
                    }                    
                }
                float infection = this.Owner.Damage.Infection;
                if (this.Owner.IsInfectionStable() && infection > 0.09f)
                {
                    if (ConfigurationManager.InfectionCommentsEnabled)
                    {
                        handler.AddValue(GD.PlayerDialog.cough_soft, 2f);
                        handler.AddValue(GD.PlayerDialog.cough_hard, 3f);
                        handler.AddValue(GD.PlayerDialog.sneeze, 2f);
                    }
                    if (infection > 0.5f)
                    {
                        isStable = false;
                    }
                }

                if (ConfigurationManager.ExplorationDialogueEnabled && isStable)
                {
                    handler.AddValue(GD.PlayerDialog.random_comment_combat_potential, 1f);
                    handler.AddValue(GD.PlayerDialog.random_comment_pure_stealth, 1f);
                    if (MiscSettings.AllowBondingDialogue)
                    {
                        if (DramaManager.Tension < 60f)
                        {
                            handler.AddValue(GD.PlayerDialog.ask_for_objective_reminder, 1f);
                            handler.AddValue(GD.PlayerDialog.idle_surroundings, 1f);
                            if (Clock.Time - DramaManager.Current.m_machine.m_changeStateTime > 120f)
                            {
                                handler.AddValue(GD.PlayerDialog.idle_group_bonding, 1f);
                            }
                        }
                    }

                    if (DramaManager.PlayersSeparated)
                    {
                        handler.AddValue(GD.PlayerDialog.group_is_not_together, 1f);
                    }
                    if (GlobalEventManager.PrisonersLost)
                    {
                        if (this.Owner.PlayerCharacterFilter != DialogCharFilter.Char_O)
                        {
                            handler.AddValue(GD.PlayerDialog.idle_no_progress, 2f);
                        }
                    }
                }

                if (handler.TryToGetBestValue(out WeightValue<uint> bestVal))
                {
                    string dialogueName = PlayerDialogDataBlock.GetBlockName(bestVal.Value);
                    if (ExtendedPlayerManager.AllPlayersInLevel.Count > 1 && !ComputerTerminalManager.AnyTerminalsPlayingAudio)
                    {
                        this.DebugPrint("Attempting to trigger dialogue : " + dialogueName);
                        this.m_owner.WantToStartDialog(bestVal.Value, false);
                    }
                    else
                    {
                        this.DebugPrint("Cannot trigger dialogue : " + dialogueName + " since there is less than 2 players..");
                    }
                }
            }

            this.StartDelayedIdleDialog();
        }




        private MinMaxTimer m_idle_delay;

        private CallBackUtils.CallBack m_idleDialogCallback;

        public CallBackUtils.CallBack<ItemEquippable> m_triggerLightOnDarkCallBack;

    }
}
