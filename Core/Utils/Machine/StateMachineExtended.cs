using System;

namespace ChatterReborn.Utils.Machine
{
    public class StateMachineExtended<MS, E> : StateMachine<MS> where MS : MachineStateBase where E : Enum
    {

        private E m_currentStateName;

        private E m_lastStateName;

        protected void SetupMachine()
        {
            m_stateEnumType = typeof(E);
            this.m_state_count = Enum.GetValues(m_stateEnumType).Length;
            this.m_states = new MS[m_state_count];
        }


        public E CurrentStateName => m_currentStateName;
        public E LastStateName => m_lastStateName;

        public override void OnCurrentState(int id)
        {
            this.m_currentStateName = (E)Enum.ToObject(m_stateEnumType, id);
        }

        public override void OnLastState(int id)
        {
            this.m_lastStateName = (E)Enum.ToObject(m_stateEnumType, id);
        }


        public void ChangeState(E id)
        {
            base.ChangeState(id);
        }

        private Type m_stateEnumType;
    }
}
