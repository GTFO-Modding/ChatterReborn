using ChatterReborn.ChatterEvent;
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
    public class DramaChatterMachine : StateMachine<DRAMA_Chatter_Base>, IChatterEventListener<PlayerDamageEvent>, IChatterEventListener<EnemyDamageEvent>, IChatterEventListener<ScoutScreamEvent>
    {
        public void Setup(PlayerAgent playerAgent)
        {
            this.DEBUG_ENABLED = DramaSettings.MachineDebugEnabled;
            this.SetupEnum<DRAMA_State>();
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
            this.WantToSyncDramaState(DramaManager.CurrentStateEnum);
            DramaChatterManager.EnableParticipation(this.CharacterID, true);
            ChatterEventListenerHandler<EnemyDamageEvent>.RegisterListener(this);
            ChatterEventListenerHandler<PlayerDamageEvent>.RegisterListener(this);
            ChatterEventListenerHandler<ScoutScreamEvent>.RegisterListener(this);

            if (this.Owner.Owner.IsBot && PlayerBotSettings.EnablePlayerMonitor)
            {
                var monitor = this.m_owner.gameObject.AddAbsoluteComponent<PlayerBotAIRootMonitor>();
                monitor.Setup(this);
                ChatterDebug.LogMessage("PlayerBotActionMonitor - added to playerID " + this.CharacterID);
            }


            m_combatMachine = new CombatStateMachine();
            m_combatMachine.Setup(playerAgent);

            foreach (var state in this.m_states)
            {
                if (state != null)
                {
                    IDRAMA_Chatter_Combat combat_state = state as IDRAMA_Chatter_Combat;
                    if (combat_state != null)
                    {
                        combat_state.SetupCombatState(m_combatMachine);
                    }
                }
            }

            this.StartState = this.GetState(DRAMA_State.ElevatorIdle);
            this.m_IsSetup = true;
        }


        private CombatStateMachine m_combatMachine;
        
       
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

            if (ConfigurationManager.VoiceIntensityAdapterEnabled && this.CurrentState is DRAMA_Chatter_Combat)
            {
                DRAMA_State switchState = DRAMA_State.Combat;

                bool timePassed = Clock.Time - this.m_changeStateTime > 10f;
                if ((DramaSettings.timePassedEnabled || timePassed) && this.Intensity < DramaSettings.enemyScoreForCombatIntensity)
                {
                    switchState = DRAMA_State.Encounter;
                }

                this.WantToSyncDramaState(switchState, false);
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

        public void Update()
        {
            if (!this.m_IsSetup)
            {
                return;
            }        

            if (this.IsLocallyOwned)
            {
                this.CheckResourcePackReceived();
            }

            m_combatMachine.UpdateState();
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

        public void ChangeState(DRAMA_State state)
        {
            m_currentDramaState = state;
            ChatterDebug.LogWarning("Changing to State " + state + " for characterID " + this.Owner.CharacterID);
            base.ChangeState(state);
        }


        private DRAMA_State m_currentDramaState;

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
        public DRAMA_State CurrentDramaState { get => m_currentDramaState; }

        public CombatData m_lastCombatData;
    }
}
