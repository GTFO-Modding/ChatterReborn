namespace ChatterReborn.Utils.Machine
{
    public abstract class MachineState
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

        public abstract void SetMachine(IStateMachine stateMachine);
        
    }

    public class MachineState<SM> : MachineState where SM : class, IStateMachine
    {
        public SM m_machine;

        public override void SetMachine(IStateMachine stateMachine)
        {
            m_machine = stateMachine as SM;
            if (m_machine == null)
            {
                throw new System.Exception("Cannot implicitly convert IStateMachine -> " + typeof(SM).Name);
            }
        }
    }
}
