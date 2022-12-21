using System;
using System.Collections;

namespace ChatterReborn.Utils.Machine
{
    public class StateMachineExtended<MS, E> : StateMachine<MS> where MS : MachineStateBase where E : Enum
    {

        protected void SetupMachine()
        {
            m_stateEnumType = typeof(E);
            IList enumArray = Enum.GetValues(m_stateEnumType);
            this.m_state_count = enumArray.Count;
            this.m_states = new MS[m_state_count];
            this.m_enumStates = new E[m_state_count];
            for (int i = 0; i < enumArray.Count; i++)
            {
                this.m_enumStates[i] = (E)enumArray[i];
            }
        }

        protected MS AddState(E stateEnum, MS stateInstance)
        {
            return this.AddState(Convert.ToInt32(stateEnum), stateInstance);
        }

        public E CurrentStateName => m_currentStateName;
        public E LastStateName => m_lastStateName;

        public override void OnCurrentState(int id)
        {
            this.m_currentStateName = this.m_enumStates[id];
        }

        public override void OnLastState(int id)
        {
            this.m_lastStateName = this.m_enumStates[id];
        }



        public void ChangeState(E state)
        {
            this.ChangeState(Convert.ToInt32(state));
        }

        public MS GetState(E stateEnum)
        {
            return this.m_states[Convert.ToInt32(stateEnum)];
        }

        private Type m_stateEnumType;

        private E[] m_enumStates;

        private E m_currentStateName;

        private E m_lastStateName;
    }
}
