using ChatterReborn.Data;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.CombatStates
{
    public class COMBAT_Regular : COMBAT_Base
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

            if (this.m_machine.GoToExtreme)
            {
                this.m_machine.ChangeState(CombatState.Extreme);
                return;
            }

            if (this.m_machine.GoToHidden)
            {
                this.m_machine.ChangeState(CombatState.Hidden);
            }
        }
    }
}
