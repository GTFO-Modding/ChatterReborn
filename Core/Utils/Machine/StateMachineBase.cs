using System;

namespace ChatterReborn.Utils.Machine
{
    public abstract class StateMachineBase
    {
        public abstract bool DEBUG_ENABLED { get; set; }
        public abstract void ChangeState(int id);
        public abstract void ChangeState(Enum enumId);
        public abstract void Reset();
    }
}
