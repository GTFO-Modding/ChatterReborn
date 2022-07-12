using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Utils.Machine
{
    public interface IStateMachine
    {
        bool DEBUG_ENABLED { get; set; }
        void ChangeState(int id);
        void ChangeState(Enum enumId);
        void Reset();
    }
}
