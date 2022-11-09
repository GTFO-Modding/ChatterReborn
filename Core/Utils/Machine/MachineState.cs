using StateMachines;

namespace ChatterReborn.Utils.Machine
{
    public abstract class MachineStateBase
    {
        public virtual void Enter() { }
        public virtual void SyncEnter() { }
        public virtual void Exit() { }
        public virtual void SyncExit() { }
        public virtual void Update() { }
        public virtual void SyncUpdate() { }
        public virtual void OnLevelCleanUp() { }
        public virtual void OnDestroyed() { }
        public virtual void Setup() { }
        public void DebugPrint(string text)
        {
            if (DEBUG_ENABLED)
            {
                ChatterDebug.LogMessage(this.GetType().Name + " - " + text);
            }
        }

        public byte ENUM_ID;

        public float StateChangeTime;

        public bool DEBUG_ENABLED { get; set; }

        public abstract void SetMachine(StateMachineBase stateMachine);
        
    }

    public abstract class MachineState<SM> : MachineStateBase where SM : StateMachineBase
    {
        public SM m_machine;

        public override void SetMachine(StateMachineBase stateMachine)
        {
            m_machine = (SM)stateMachine;
        }
    }
}
