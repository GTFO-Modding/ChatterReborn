using ChatterReborn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.CombatStates
{
    public class COMBAT_Hidden : COMBAT_Base
    {
        public override void Update()
        {
            CommonUpdate();
        }

        public override void SyncUpdate()
        {
            CommonUpdate();
        }

        private void CommonUpdate()
        {
            if (!this.m_machine.IsInCombat)
            {
                this.m_machine.ChangeState(CombatState.None);
                return;
            }

            if (this.m_machine.GoToRegular)
            {
                this.m_machine.ChangeState(CombatState.Regular);
            }
        }
    }
}
