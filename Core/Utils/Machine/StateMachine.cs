using System;

namespace ChatterReborn.Utils.Machine
{
    public class StateMachine<S> : StateMachineBase where S : MachineStateBase
    {

        public override bool DEBUG_ENABLED
        {
            get
            {
                return this.m_debug_enabled;
            }
            set
            {
                m_debug_enabled = true;
                if (this.m_states != null)
                {
                    foreach (var state in this.m_states)
                    {
                        if (state != null)
                        {
                            state.DEBUG_ENABLED = value;
                        }
                    }
                }
                
            }
        }

        public virtual bool IsLocallyOwned => true;
        protected S AddState(int stateID, S instance)
        {
            instance.DEBUG_ENABLED = this.DEBUG_ENABLED;            
            instance.SetMachine(this);
            instance.ENUM_ID = (byte)stateID;
            this.m_states[stateID] = instance;
            instance.Setup();
            return instance;
        }

        protected virtual S AddState(Enum stateEnum, S stateInstance)
        {
            int id = Convert.ToInt32(stateEnum);
            return this.AddState(id, stateInstance);
        }

        protected void SetupEnum<E>() where E : Enum
        {
            this.m_state_count = Enum.GetValues(typeof(E)).Length;

            this.m_states = new S[m_state_count];
        }

        public override void ChangeState(Enum state)
        {
            this.ChangeState(Convert.ToInt32(state));
        }

        public override void ChangeState(int stateID)
        {
            this.ChangeState(this.m_states[stateID]);
        }

        public void ChangeState(S state)
        {
            this.m_last_state = this.m_current_state;
            this.m_current_state = state;
            this.m_current_state.StateChangeTime = Clock.Time;
            this.m_changeStateTime = Clock.Time;
            if (this.IsLocallyOwned)
            {
                if (this.m_last_state != null)
                {
                    this.m_last_state.Exit();                    
                }

                this.m_current_state.Enter();
            }
            else
            {
                if (this.m_last_state != null)
                {
                    this.m_last_state.SyncExit();
                }
                this.m_current_state.SyncEnter();
            }

        }

        public virtual void UpdateState()
        {
            this.m_localDelta = Clock.Time - this.m_localDeltaRef;
            this.m_localDeltaRef = Clock.Time;
            if (this.IsLocallyOwned)
            {
                this.m_current_state.Update();
                return;
            }
            this.m_current_state.SyncUpdate();
        }



        public virtual void OnLevelCleanUp()
        {
        }

        public virtual void OnDestroyed()
        {

        }

        public S StartState
        {
            get
            {
                return this.m_start_state;
            }
            set
            {
                this.m_start_state = value;
                this.ChangeState(value);
            }
        }

        public override void Reset()
        {
            this.ChangeState(this.m_start_state);
        }



        public S GetState(int id)
        {
            return this.m_states[id];
        }

        public S GetState(Enum stateEnum)
        {
            return this.m_states[Convert.ToInt32(stateEnum)];
        }

        public S[] m_states;

        public S m_last_state;

        public S m_current_state = Activator.CreateInstance<S>();

        private S m_start_state;

        public float m_changeStateTime;

        protected bool m_IsSetup;

        public int m_state_count;

        protected float m_localDeltaRef;
        public float m_localDelta;

        private bool m_debug_enabled;
    }
}
