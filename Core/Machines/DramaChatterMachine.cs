﻿using ChatterReborn.ChatterEvent;
using ChatterReborn.Components;
using ChatterReborn.Data;
using ChatterReborn.Drama_Chatter_States;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using ChatterReborn.Utils.Machine;
using ChatterRebornSettings;
using GameData;
using Player;
using SNetwork;
using UnityEngine;

namespace ChatterReborn.Machines
{
    public class DramaChatterMachine : StateMachineExtended<DRAMA_Chatter_Base, DRAMA_State>, IChatterEventListener<PlayerDamageEvent>, IChatterEventListener<EnemyDamageEvent>, IChatterEventListener<ScoutScreamEvent>
    {
        public void Setup(PlayerAgent playerAgent)
        {
            this.DEBUG_ENABLED = Settings.Drama.MachineDebugEnabled;
            this.SetupMachine();
            this.AddState(DRAMA_State.ElevatorIdle, new DRAMA_Chatter_ElevatorIdle());
            this.AddState(DRAMA_State.ElevatorGoingDown, new DRAMA_Chatter_ElevatorGoingDown());
            this.AddState(DRAMA_State.Exploration, new DRAMA_Chatter_Exploration());
            this.AddState(DRAMA_State.Alert, new DRAMA_Chatter_Alert());
            this.AddState(DRAMA_State.Sneaking, new DRAMA_Chatter_Sneaking());
            this.AddState(DRAMA_State.Encounter, new DRAMA_Chatter_Encounter());
            this.AddState(DRAMA_State.Combat, new DRAMA_Chatter_Combat());
            this.AddState(DRAMA_State.IntentionalCombat, new DRAMA_Chatter_IntentionalCombat());
            this.AddState(DRAMA_State.Survival, new DRAMA_Chatter_Survival());
            this.Owner = playerAgent;
            this.CharacterID = playerAgent.CharacterID;
            
            

            SetupBaseIntensity();
            SetupEnemyIntensity();
            SetupHurtIntensity();
            
            DramaChatterManager.Current.m_updateAction += this.Update;
            
            DramaChatterManager.EnableParticipation(this.CharacterID, true);
            ChatterEventListenerHandler<EnemyDamageEvent>.RegisterListener(this);
            ChatterEventListenerHandler<PlayerDamageEvent>.RegisterListener(this);
            ChatterEventListenerHandler<ScoutScreamEvent>.RegisterListener(this);

            if (this.Owner.Owner.IsBot && Settings.PlayerBot.EnablePlayerMonitor)
            {
                var monitor = this.m_owner.gameObject.AddAbsoluteComponent<PlayerBotAIRootMonitor>();
                monitor.Setup(this);
                ChatterDebug.LogMessage("PlayerBotActionMonitor - added to playerID " + this.CharacterID);
            }



            this.StartState = this.GetState(DRAMA_State.ElevatorIdle);
            this.m_IsSetup = true;
        }


        
       
        private CallBackUtils.CallBack m_thanksCallBack;
        private void SetupEnemyIntensity()
        {
            this.m_IntEnemyCurr = 0f;
            this.m_IntEnemyInc = 8f;
            this.m_IntEnemyDec = 2f;
            this.m_IntEnemyRange = 5f;
            this.m_IntEnemyRangeWithTag = 15f;
        }

        private void SetupBaseIntensity()
        {
            this.m_updateIntTimer = 0f;
            this.m_updateFirstUpdate = false;
            this.m_updateIntIncDelay = 0.5f;
        }
        private void SetupHurtIntensity()
        {
            this.m_IntHurtCurr = 0f;
            this.m_IntHurtInc = 8f;
            this.m_IntHurtDec = 2f;
            this.m_IntHurtMax = 50f;
        }

        public override bool IsLocallyOwned => this.Owner.IsLocallyOwned;




        public void WantToSyncDramaState(DRAMA_State state, bool force = false)
        {
            bool canSync = false;
            if (ConfigurationManager.VoiceIntensityAdapterEnabled && this.AllowedToParticipate)
            {
                canSync = true;
            }
            if (force || canSync)
            {
                DramaManager.WantToSyncState(this.Owner.CharacterID, state, DramaManager.Tension);
            }
        }



        private bool IsLowHealth => this.m_owner.Damage.GetHealthRel() < 0.2f;

        public float Intensity
        {
            get
            {
                return Mathf.Clamp(this.m_IntHurtCurr + this.m_IntEnemyCurr, 0, 100f);
            }
        }

        private void UpdateIntensity()
        {
            if (!m_updateFirstUpdate || m_updateIntTimer < Clock.Time)
            {
                m_updateFirstUpdate = true;
                m_updateIntTimer = Clock.Time + m_updateIntIncDelay;
                if (this.IsLowHealth)
                {
                    m_IntHurtCurr = Mathf.Min(this.m_IntHurtCurr + this.m_IntHurtInc, this.m_IntHurtMax);
                }
                else
                {
                    m_IntHurtCurr = Mathf.Max(this.m_IntHurtCurr - this.m_IntHurtDec, 0f);
                }

                float max_intensity = EnemyDetectionManager.Current.m_enemyScores[this.CharacterID];
                if (m_IntEnemyCurr < max_intensity)
                {
                    m_IntEnemyCurr = Mathf.Min(100f, m_IntEnemyCurr + m_IntEnemyInc);
                }
                else
                {
                    m_IntEnemyCurr = Mathf.Max(0f, m_IntEnemyCurr - m_IntEnemyDec);
                }
            }
        }

        private void UpdateVoiceIntensity()
        {
            if (!StaticGlobalManager.VoiceIntensityAdapterEnabled)
            {
                return;
            }

            if (ConfigurationManager.VoiceIntensityAdapterEnabled && this.m_current_state.IsCombatState)
            {
                DRAMA_State switchState = DRAMA_State.Combat;

                bool timePassed = Clock.Time - this.m_changeStateTime > 10f;
                if ((Settings.Drama.timePassedEnabled || timePassed) && this.Intensity < Settings.Drama.enemyScoreForCombatIntensity)
                {
                    switchState = DRAMA_State.Encounter;
                }

                this.WantToSyncDramaState(switchState, false);
            }
        }

        private void UpdateDebugRecallDramaState()
        {
            var recallData = new pDramaState
            {
                dramaState = (int)DRAMA_State.ElevatorIdle,
                tension = DramaManager.Tension,
                playerID = this.CharacterID
            };

            bool hasRecall = false;
            if (Input.GetKeyDown(KeyCode.F1))
            {
                recallData.dramaState = (int)DRAMA_State.Exploration;
                hasRecall = true;
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                recallData.dramaState = (int)DRAMA_State.Sneaking;
                hasRecall = true;
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                recallData.dramaState = (int)DRAMA_State.Encounter;
                hasRecall = true;
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                recallData.dramaState = (int)DRAMA_State.Combat;
                hasRecall = true;
            }

            else if (Input.GetKeyDown(KeyCode.F5))
            {
                UnityEngine.Debug.Log("My state : " + DramaManager.SyncedPlayerStates[this.CharacterID]);
            }

            if (hasRecall)
            {
                DramaManager.Current.DoRecallDramaState(recallData);
            }
            
        }
        private void UpdateLocal()
        {
            UpdateResponseKeyBinds();
            CheckResourcePackReceived();
        }

        private void UpdateResponseKeyBinds()
        {
            if (FocusStateManager.CurrentState != eFocusState.FPS)
            {
                return;
            }

            if (CoolDownManager.HasCooldown(CoolDownType.QuickResponse))
            {
                return;
            }

            
            if (Input.GetKeyDown(ConfigurationManager.QuickAffirmativeResponse))
            {
                if (ConfigurationManager.QuickAffirmativeResponse != KeyCode.None)
                {
                    WeightHandler<uint> affirmHandler = WeightHandler<uint>.CreateWeightHandler();
                    affirmHandler.AddValue(GD.PlayerDialog.CL_Yes, 1f);
                    if (this.Owner.PlayerCharacterFilter != DialogCharFilter.Char_F)
                    {
                        affirmHandler.AddValue(GD.PlayerDialog.CL_WillDo, 5f);
                    }
                    affirmHandler.AddValue(GD.PlayerDialog.CL_IWillDoIt, 2f);

                    this.Owner.WantToStartDialog(affirmHandler.Best.Value, true);
                    CoolDownManager.ApplyCooldown(CoolDownType.QuickResponse, 2f);
                }
                
            }
            else if (Input.GetKeyDown(ConfigurationManager.QuickNegatoryResponse))
            {
                if (ConfigurationManager.QuickNegatoryResponse != KeyCode.None)
                {
                    this.Owner.WantToStartDialog(GD.PlayerDialog.CL_ICantDoThat, true);
                    CoolDownManager.ApplyCooldown(CoolDownType.QuickResponse, 2f);
                }                
            }
        }



        private void CheckResourcePackReceived()
        {
            var communicator = this.Owner.m_interactionCommunicator;
            if (communicator == null)
            {
                return;
            }
            var data = communicator.m_lastReceivedData;
            if (data.Progress == 1f)
            {
                if (this.m_thanksCallBack == null)
                {
                    this.m_thanksCallBack = new CallBackUtils.CallBack(SayThankYou);
                    this.m_thanksCallBack.QueueCallBack(1.25f);
                }
            }
        }


        private void SayThankYou()
        {    
            if (ConfigurationManager.ResourcePackRecievedConfirmationEnabled)
            {
                this.Owner.WantToStartDialog(GD.PlayerDialog.CL_ThankYou, true);
            }
            this.m_thanksCallBack = null;
        }


        private void UpdateCombatState()
        {
            if (this.HasLastCombatData)
            {
                if (DramaManager.CurrentStateEnum != DRAMA_State.Alert && DramaManager.CurrentStateEnum != DRAMA_State.Combat)
                {
                    this.HasLastCombatData = false;
                }
            }
        }

        private void UpdateCheckHealth()
        {
            float currentHealth = this.Owner.Damage.GetHealthRel();
            if (!m_firstHealthCheck)
            {
                m_firstHealthCheck = true;
                this.m_lastKnownHealth = currentHealth;
                return;
            }


            if (currentHealth != this.m_lastKnownHealth)
            {                
                if (currentHealth < this.m_lastKnownHealth)
                {
                    float damage = this.m_lastKnownHealth - currentHealth;
                    ChatterEventListenerHandler<PlayerDamageEvent>.PostEvent(new PlayerDamageEvent
                    {
                        damageAmount = damage,
                        damageReceiver = this.Owner,
                    });
                }

                this.m_lastKnownHealth = currentHealth;
            }

        }


        private float m_lastKnownHealth;

        private bool m_firstHealthCheck;

        public void Update()
        {
            if (!this.m_IsSetup)
            {
                return;
            }

            this.UpdateCheckHealth();

            if (this.IsLocallyOwned)
            {
                this.UpdateLocal();
            }

            
            this.UpdateCombatState();
            this.UpdateIntensity();
            this.UpdateVoiceIntensity();            
            this.UpdateState();
        }

        public DRAMA_Chatter_Base CurrentState
        {
            get
            {
                return this.m_current_state;
            }
        }

        public new void ChangeState(DRAMA_State state)
        {
            ChatterDebug.LogWarning("Changing to State " + state + " for characterID " + this.Owner.CharacterID);
            base.ChangeState(state);
        }



        public float LastStateChangeTime
        {
            get
            {
                return this.m_changeStateTime;
            }
        }

        public override void OnLevelCleanUp()
        {
            
            for (int i = 0; i < this.m_states.Length; i++)
            {
                var state = this.m_states[i];
                if (state != null)
                {
                    state.OnLevelCleanUp();
                }
            }

            this.HasLastCombatData = false;
        }

        public override void OnDestroyed()
        {
            
            foreach (var state in this.m_states)
            {
                if (state != null)
                {
                    state.OnDestroyed();
                }
            }

            if (this.m_thanksCallBack != null)
            {
                this.m_thanksCallBack.RemoveCallBack();
                this.m_thanksCallBack = null;
            }

            this.HasLastCombatData = false;
            ChatterEventListenerHandler<EnemyDamageEvent>.DeRegisterListener(this);
            ChatterEventListenerHandler<PlayerDamageEvent>.DeRegisterListener(this);
            ChatterEventListenerHandler<ScoutScreamEvent>.DeRegisterListener(this);

            this.WantToSyncDramaState(DRAMA_State.ElevatorIdle, true);
            DramaChatterManager.EnableParticipation(this.CharacterID, false);
            DramaChatterManager.Current.m_updateAction -= this.Update;
        }


        public void OnChatterEvent(PlayerDamageEvent playerDamageEvent)
        {
            if (playerDamageEvent.damageReceiver?.CharacterID == this.m_owner.CharacterID)
            {
                this.m_current_state.OnLocalDamage(playerDamageEvent.damageAmount);
            }
            else
            {
                this.m_current_state.OnTeammatesDamage(playerDamageEvent.damageAmount);
            }
        }

        public void OnChatterEvent(EnemyDamageEvent chatterEvent)
        {
            PlayerAgent killer = chatterEvent.m_attacker?.TryCast<PlayerAgent>();
            if (killer == null || killer.CharacterID != this.m_owner.CharacterID)
            {
                return;
            }

            this.m_current_state.OnDamagedEnemy(chatterEvent);
        }

        public void OnChatterEvent(ScoutScreamEvent chatterEvent)
        {
            ((DRAMA_Chatter_Combat)this.GetState(DRAMA_State.Combat)).ScoutScreamed();
        }


        public PlayerAgent Owner
        {
            get
            {
                return this.m_owner;
            }
            set
            {
                this.m_owner = value;
                for (int i = 0; i < this.m_states.Length; i++)
                {
                    var state = this.m_states[i];
                    if (state != null)
                    {
                        state.SetOwner(value);
                    }
                }
            }
        }

        public int CharacterID { get; private set; }

        public bool AllowedToParticipate
        {
            get
            {
                if (this.IsLocallyOwned)
                {
                    return true;
                }
                if (this.m_owner.IsBotOwned())
                {
                    return ConfigurationManager.AllowBotsToParticipateEnabled;
                }
                return SNet.IsMaster && !this.m_owner.Owner.IsBot && DramaChatterManager.IsAllowedToParticipate(this.CharacterID);
            }
        }

        private float m_updateIntTimer;
        private bool m_updateFirstUpdate;
        private float m_updateIntIncDelay;
        private float m_IntEnemyCurr;
        private float m_IntEnemyInc;
        private float m_IntEnemyDec;
        private float m_IntEnemyRange;
        private float m_IntHurtCurr;
        private float m_IntHurtInc;
        private float m_IntHurtDec;
        private float m_IntHurtMax;
        private float m_IntEnemyRangeWithTag;
        private PlayerAgent m_owner;

        public bool HasLastCombatData { get; set; }

        public CombatData m_lastCombatData;
    }
}
