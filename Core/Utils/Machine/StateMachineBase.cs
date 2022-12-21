using System;

namespace ChatterReborn.Utils.Machine
{
    public abstract class StateMachineBase
    {
        public abstract bool DEBUG_ENABLED { get; set; }
        public abstract void ChangeState(int id);
        public abstract void Reset();
        public abstract void OnCurrentState(int id);
        public abstract void OnLastState(int id);

        public virtual bool IsLocallyOwned => true;

        public float m_changeStateTime;

        protected bool m_IsSetup;

        public int m_state_count;

        protected float m_localDeltaRef;

        public float m_localDelta;

    }
}
