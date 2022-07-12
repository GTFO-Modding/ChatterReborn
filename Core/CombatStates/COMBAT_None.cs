using ChatterReborn.Data;

namespace ChatterReborn.CombatStates
{
    public class COMBAT_None : COMBAT_Base
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
            if (this.m_machine.IsInCombat)
            {
                this.m_machine.ChangeState(this.m_machine.GoToExtreme ? CombatState.Extreme : this.m_machine.GoToRegular ? CombatState.Regular : CombatState.Hidden);
            }
        }
    }
}
