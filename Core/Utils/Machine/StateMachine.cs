using System;

namespace ChatterReborn.Utils.Machine
{
    public abstract class StateMachine<MS> : StateMachineBase where MS : MachineStateBase
    {

        public override bool DEBUG_ENABLED
        {
            get
            {
                return this.m_debug_enabled;
            }
            set
            {
                m_debug_enabled = value;
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

        protected MS AddState(int stateID, MS instance)
        {
            instance.DEBUG_ENABLED = this.DEBUG_ENABLED;            
            instance.SetMachine(this);
            instance.ENUM_ID = (byte)stateID;
            this.m_states[stateID] = instance;
            instance.Setup();
            return instance;
        }

        protected virtual MS AddState(Enum stateEnum, MS stateInstance)
        {
            int id = Convert.ToInt32(stateEnum);
            return this.AddState(id, stateInstance);
        }



        public override void ChangeState(Enum state)
        {
            this.ChangeState(Convert.ToInt32(state));
        }

        public override void ChangeState(int stateID)
        {
            this.ChangeState(this.m_states[stateID]);
        }

        public void ChangeState(MS state)
        {
            this.m_last_state = this.m_current_state;

            if (this.m_last_state != null)
            {
                this.OnLastState(Convert.ToInt32(this.m_last_state.ENUM_ID));
            }
            
            this.m_current_state = state;
            this.m_current_state.StateChangeTime = Clock.Time;
            this.m_changeStateTime = Clock.Time;

            this.OnCurrentState(Convert.ToInt32(this.m_current_state.ENUM_ID));

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

        public MS StartState
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
            if (this.m_start_state != null)
            {
                this.ChangeState(this.m_start_state);
            }            
        }



        public MS GetState(int id)
        {
            return this.m_states[id];
        }

        public MS GetState(Enum stateEnum)
        {
            return this.m_states[Convert.ToInt32(stateEnum)];
        }


        public MS[] m_states;

        public MS m_last_state;

        public MS m_current_state = Activator.CreateInstance<MS>();

        private MS m_start_state;

        private bool m_debug_enabled;
    }
}
