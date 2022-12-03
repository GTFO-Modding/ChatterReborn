using StateMachines;

namespace ChatterReborn.Utils.Machine
{   

    public abstract class MachineState<SM> : MachineStateBase where SM : StateMachineBase
    {
        public SM m_machine;

        public override void SetMachine(StateMachineBase stateMachine)
        {
            m_machine = (SM)stateMachine;
        }
    }
}
